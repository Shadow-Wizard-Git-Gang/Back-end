using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using VC.Helpers.Exceptions;
using VC.Models.DTOs.UserDTOs;
using VC.Models.Identity;
using VC.Models;
using VC.Services;
using Xunit;
using VC.Tests.Data;

namespace VC.Tests.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly UserCreateRequestDTO _userCreateRequest;
        private readonly ApplicationUser _appUser;
        private readonly User _user;

        public UserServiceTests()
        {
            _userManagerMock = MockingHelpers.CreateUserManagerMock();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userManagerMock.Object, _mapperMock.Object);

            _userCreateRequest = TestDataHelper.CreateUserCreateRequestDTO();
            _appUser = TestDataHelper.CreateApplicationUser();
            _user = TestDataHelper.CreateUser();
        }

        [Fact]
        public async Task CreateUserAsync_ReturnUser_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            _mapperMock.Setup(x => x.Map<ApplicationUser>(_userCreateRequest)).Returns(_appUser);
            _userManagerMock.Setup(x => x.CreateAsync(_appUser, _userCreateRequest.Password)).ReturnsAsync(IdentityResult.Success);
            _mapperMock.Setup(x => x.Map<User>(_appUser)).Returns(_user);

            // Act
            var result = await _userService.CreateUserAsync(_userCreateRequest);

            // Assert
            Assert.Equal(_user, result);
        }

        [Fact]
        public async Task CreateUserAsync_ReturnThrowSignUpServiceException_WhenUserIsNotCreatedSuccessfully()
        {
            // Arrange
            var errors = new List<IdentityError>
            {
                new IdentityError
                {
                    Description = "Error 1"
                },
                new IdentityError
                {
                    Description = "Error 2"
                }
            };
            _mapperMock.Setup(x => x.Map<ApplicationUser>(_userCreateRequest)).Returns(_appUser);
            _userManagerMock.Setup(x => x.CreateAsync(_appUser, _userCreateRequest.Password)).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var exception = await Assert.ThrowsAsync<SignUpServiceException>(() => _userService.CreateUserAsync(_userCreateRequest));

            // Assert
            Assert.Equal("Error 1\nError 2\n", exception.Message);
        }

    }
}
