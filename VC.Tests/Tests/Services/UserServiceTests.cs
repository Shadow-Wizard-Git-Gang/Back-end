using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserCreateRequestDTO _userSignUpRequest;
        private readonly ApplicationUser _appUser;
        private readonly User _user;

        public UserServiceTests()
        {
            _mockUserManager = MockingHelpers.CreateUserManagerMock();
            _mockMapper = new Mock<IMapper>();
            _userService = new UserService(_mockUserManager.Object, _mockMapper.Object);
            _userSignUpRequest = new UserCreateRequestDTO();
            _appUser = TestDataHelper.CreateApplicationUser();
            _user = TestDataHelper.CreateUser();
        }

        [Fact]
        public async Task CreateUserAsync_ReturnUser_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            
            _mockMapper.Setup(x => x.Map<ApplicationUser>(_userSignUpRequest)).Returns(_appUser);
            _mockUserManager.Setup(x => x.CreateAsync(_appUser, _userSignUpRequest.Password)).ReturnsAsync(IdentityResult.Success);
            _mockMapper.Setup(x => x.Map<User>(_appUser)).Returns(_user);

            // Act
            var result = await _userService.CreateUserAsync(_userSignUpRequest);

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
            _mockMapper.Setup(x => x.Map<ApplicationUser>(_userSignUpRequest)).Returns(_appUser);
            _mockUserManager.Setup(x => x.CreateAsync(_appUser, _userSignUpRequest.Password)).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var exception = await Assert.ThrowsAsync<SignUpServiceException>(() => _userService.CreateUserAsync(_userSignUpRequest));

            // Assert
            Assert.Equal("Error 1\r\nError 2\r\n", exception.Message);
        }

    }
}
