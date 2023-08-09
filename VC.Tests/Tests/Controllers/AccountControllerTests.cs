using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VC.Controllers;
using VC.Models.DTOs.AccountDTOs;
using VC.Services.IServices;
using VC.Tests.Data;
using Xunit;

namespace VC.Tests.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly AccountController _accountController;

        private readonly UserSignInRequestDTO _userSignInRequest;
        private readonly UserSignInResponseDTO _userSignInResponse;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _accountController = new AccountController(_accountServiceMock.Object);

            _userSignInRequest = TestDataHelper.CreateUserSignInRequestDTO();
            _userSignInResponse = TestDataHelper.CreateUserSignInResponseDTO();
        }

        [Fact]
        public async Task SignInAsync_ReturnsUnauthorized_WhenInvalidCredentialsAreProvided()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.SignInAsync(It.IsAny<UserSignInRequestDTO>())).ReturnsAsync((UserSignInResponseDTO?)null);

            // Act
            var result = await _accountController.SignInAsync(_userSignInRequest);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid Email or Password", (result as UnauthorizedObjectResult)?.Value);
        }

        [Fact]
        public async Task SignInAsync_ReturnsOk_WhenValidCredentialsAreProvided()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.SignInAsync(It.IsAny<UserSignInRequestDTO>())).ReturnsAsync(_userSignInResponse);

            // Act
            var result = await _accountController.SignInAsync(_userSignInRequest);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_userSignInResponse, (result as OkObjectResult)?.Value);
        }

        [Fact]
        public async Task SignInAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.SignInAsync(It.IsAny<UserSignInRequestDTO>())).ThrowsAsync(new Exception());

            // Act
            var result = await _accountController.SignInAsync(_userSignInRequest);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, (result as StatusCodeResult)?.StatusCode);
        }
    }
}
