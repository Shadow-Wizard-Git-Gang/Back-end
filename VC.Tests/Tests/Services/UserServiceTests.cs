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
using VC.Services.IServices;

namespace VC.Tests.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAccountService> _accountServiceMock;

        private readonly UserCreateRequestDTO _userCreateRequest;
        private readonly ApplicationUser _appUser;
        private readonly User _user;

        public UserServiceTests()
        {
            _userManagerMock = MockingHelpers.CreateUserManagerMock();
            _mapperMock = new Mock<IMapper>();
            _accountServiceMock = new Mock<IAccountService>();
            _userService = new UserService(_userManagerMock.Object, _accountServiceMock.Object, _mapperMock.Object);

            _userCreateRequest = new UserCreateRequestDTO { 
                Email = "test@test.com", 
                Password = "123456" 
            };
            _appUser = new ApplicationUser { 
                Email = _userCreateRequest.Email
            };
            _user = new User { 
                Email = _userCreateRequest.Email 
            };
        }

        [Fact]
        public async Task CreateUserAsync_ReturnUserAndSendConfirmationLetter_WhenUserIsCreatedSuccessfully()
        {
            // Arrange

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _accountServiceMock.Setup(x => x.SendConfirmationLetterAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<ApplicationUser>(It.IsAny<UserCreateRequestDTO>()))
                .Returns(_appUser);
            _mapperMock.Setup(x => x.Map<User>(It.IsAny<ApplicationUser>()))
                .Returns(_user);

            // Act
            var result = await _userService.CreateUserAsync(_userCreateRequest);

            // Assert
            Assert.Equal(_user.Email, result.Email);
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _accountServiceMock.Verify(x => x.SendConfirmationLetterAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ApplicationUser>(It.IsAny<UserCreateRequestDTO>()), Times.Once);
            _mapperMock.Verify(x => x.Map<User>(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ThrowSignUpServiceException_WhenUserManagerCreateAsyncFailed()
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

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));
            _mapperMock.Setup(x => x.Map<ApplicationUser>(It.IsAny<UserCreateRequestDTO>()))
                .Returns(_appUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SignUpServiceException>(() => _userService.CreateUserAsync(_userCreateRequest));
            
            Assert.Equal("Error 1\nError 2\n", exception.Message);

            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _accountServiceMock.Verify(x => x.SendConfirmationLetterAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _mapperMock.Verify(x => x.Map<ApplicationUser>(It.IsAny<UserCreateRequestDTO>()), Times.Once);
            _mapperMock.Verify(x => x.Map<User>(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAsync_ThrowSignUpServiceException_WhenAccountServiceSendConfirmationLetterAsyncFailed()
        {
            // Arrange
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>()));
            _accountServiceMock.Setup(x => x.SendConfirmationLetterAsync(It.IsAny<ApplicationUser>()))
                .Throws<Exception>();
            _mapperMock.Setup(x => x.Map<ApplicationUser>(It.IsAny<UserCreateRequestDTO>()))
                .Returns(_appUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<SignUpServiceException>(() => _userService.CreateUserAsync(_userCreateRequest));

            Assert.Equal("Problem with sending confirmation letter", exception.Message);

            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _accountServiceMock.Verify(x => x.SendConfirmationLetterAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ApplicationUser>(It.IsAny<UserCreateRequestDTO>()), Times.Once);
            _mapperMock.Verify(x => x.Map<User>(It.IsAny<ApplicationUser>()), Times.Never);
        }

    }
}
