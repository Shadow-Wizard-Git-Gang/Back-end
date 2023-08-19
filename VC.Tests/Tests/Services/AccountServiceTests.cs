using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Moq;
using VC.Helpers.JWT;
using VC.Models;
using VC.Models.DTOs.AccountDTOs;
using VC.Models.Identity;
using VC.Services;
using VC.Services.IServices;
using VC.Tests.Data;
using Xunit;

namespace VC.Tests.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<IJwtGenerator> _jwtGeneratorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        private readonly AccountService _accountService;

        private readonly UserSignInRequestDTO _userSignInRequest;
        private readonly ApplicationUser _appUser;
        private readonly User _user;

        private readonly ObjectId _objectId;


        public AccountServiceTests()
        {
            _userManagerMock = MockingHelpers.CreateUserManagerMock();
            _signInManagerMock = MockingHelpers.CreateSignInManagerMock(_userManagerMock.Object);

            _jwtGeneratorMock = new Mock<IJwtGenerator>();
            _mapperMock = new Mock<IMapper>();
            _configurationMock = new Mock<IConfiguration>();
            _emailServiceMock = new Mock<IEmailService>();

            _accountService = new AccountService(
                _signInManagerMock.Object,
                _userManagerMock.Object,
                _configurationMock.Object,
                _jwtGeneratorMock.Object,
                _emailServiceMock.Object,
                _mapperMock.Object);

            _objectId = new ObjectId();

            _userSignInRequest = new UserSignInRequestDTO
            {
                Email = "test@test.com",
                Password = "password"
            };
            _appUser = new ApplicationUser
            {
                Id = _objectId,
                Email = _userSignInRequest.Email,
                EmailConfirmed = true
            };
            _user = new User
            {
                Id = _objectId.ToString(),
                Email = _userSignInRequest.Email,
                Password = _userSignInRequest.Password
            };
        }

        [Fact]
        public async Task SignInAsync_ReturnsNull_WhenUserNotFound()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _accountService.SignInAsync(_userSignInRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SignInAsync_ReturnsNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(_appUser);
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _accountService.SignInAsync(_userSignInRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SignInAsync_ReturnsUserSignInResponseDTO_WhenPasswordIsCorrectAndEmailConfirmed()
        {
            // Arrange
            var token = "token";

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(_appUser);
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
            _mapperMock.Setup(x => x.Map<User>(It.IsAny<ApplicationUser>()))
                .Returns(_user);
            _jwtGeneratorMock.Setup(x => x.CreateToken(It.IsAny<ApplicationUser>()))
                .Returns(token);

            // Act
            var result = await _accountService.SignInAsync(_userSignInRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_user, result.User);
            Assert.Equal(token, result.Token);
        }

        [Fact]
        public async Task SignInAsync_ReturnsNull_WhenUserIsFoundAndEmailNotConfirmed()
        {
            // Arrange
            var token = "token";

            _appUser.EmailConfirmed = false;

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(_appUser);

            // Act
            var result = await _accountService.SignInAsync(_userSignInRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SendConfirmationLetterAsync_SendsConfirmationLetter()
        {
            // Arrange
            _userManagerMock
                .Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("token");

            _configurationMock
                .Setup(c => c["URLToTheConfirmationPage"])
                .Returns("url/{0}/{1}");

            _configurationMock
                .Setup(c => c["OrganizationEmail"])
                .Returns("organization@email.com");

            // Act
            await _accountService.SendConfirmationLetterAsync(_appUser);

            // Assert
            _emailServiceMock.Verify(
                es => es.SendAsync(
                    "organization@email.com",
                    "test@test.com",
                    "Please confirm your email",
                    $"Please click on this link to confirm your email address: url/{_appUser.Id}/token"),
                Times.Once);
        }
        [Fact]
        public async Task ConfirmEmailAsync_ReturnsTrue_WhenUserIsFound()
        {
            // Arrange
            var userId = "1";
            var token = "token";

            _userManagerMock
                .Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_appUser);

            _userManagerMock
                .Setup(um => um.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountService.ConfirmEmailAsync(userId, token);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsFalse_WhenUserIsNotFound()
        {
            // Arrange
            var userId = "1";
            var token = "token";

            _userManagerMock
                .Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _accountService.ConfirmEmailAsync(userId, token);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsFalse_WhenConfirmationFailed()
        {
            // Arrange
            var userId = "1";
            var token = "token";

            _userManagerMock
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_appUser);

            _userManagerMock
                .Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _accountService.ConfirmEmailAsync(userId, token);

            // Assert
            Assert.False(result);
        }
    }
}
