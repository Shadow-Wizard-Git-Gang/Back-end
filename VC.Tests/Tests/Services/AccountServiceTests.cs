using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using VC.Helpers.JWT;
using VC.Models.DTOs.AccountDTOs;
using VC.Models.Identity;
using VC.Models;
using VC.Services;
using Xunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using VC.Tests.Data;

namespace VC.Tests.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<IJwtGenerator> _jwtGeneratorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AccountService _accountService;

        private readonly UserSignInRequestDTO _userSignInRequest;
        private readonly ApplicationUser _appUser;
        private readonly User _user;
        

        public AccountServiceTests()
        {
            _userManagerMock = MockingHelpers.CreateUserManagerMock();
            _signInManagerMock = MockingHelpers.CreateSignInManagerMock(_userManagerMock.Object);

            _jwtGeneratorMock = new Mock<IJwtGenerator>();
            _mapperMock = new Mock<IMapper>();
            _accountService = new AccountService(
                _signInManagerMock.Object,
                _userManagerMock.Object,
                _jwtGeneratorMock.Object,
                _mapperMock.Object);

            _userSignInRequest = TestDataHelper.CreateUserSignInRequestDTO();
            _appUser = TestDataHelper.CreateApplicationUser();
            _user = TestDataHelper.CreateUser();
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
        public async Task SignInAsync_ReturnsUserSignInResponseDTO_WhenPasswordIsCorrect()
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
    }
}
