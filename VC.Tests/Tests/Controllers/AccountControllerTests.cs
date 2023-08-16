using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VC.Controllers;
using VC.Models.DTOs.AccountDTOs;
using VC.Services.IServices;
using Xunit;

namespace VC.Tests.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly AccountController _accountController;

        private readonly UserSignInRequestDTO _userSignInRequest;
        private readonly UserSignInResponseDTO _userSignInResponse;
        private readonly UserConfirmationEmailRequest _userConfirmationEmailRequest;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _accountController = new AccountController(_accountServiceMock.Object);

            _userSignInRequest = new UserSignInRequestDTO();
            _userSignInResponse = new UserSignInResponseDTO();
            _userConfirmationEmailRequest = new UserConfirmationEmailRequest {
                UserId = "123",
                Token = "abc"
            };
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

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsOk_WhenConfirmationIsSuccessful()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.ConfirmEmailAsync(_userConfirmationEmailRequest.UserId, _userConfirmationEmailRequest.Token))
                .ReturnsAsync(true);
            var controller = new AccountController(_accountServiceMock.Object);

            // Act
            var result = await controller.ConfirmEmailAsync(_userConfirmationEmailRequest);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsBadRequest_WhenConfirmationIsUnsuccessful()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.ConfirmEmailAsync(_userConfirmationEmailRequest.UserId, _userConfirmationEmailRequest.Token))
                .ReturnsAsync(false);
            var controller = new AccountController(_accountServiceMock.Object);

            // Act
            var result = await controller.ConfirmEmailAsync(_userConfirmationEmailRequest);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.ConfirmEmailAsync(_userConfirmationEmailRequest.UserId, _userConfirmationEmailRequest.Token))
                .ThrowsAsync(new Exception());
            var controller = new AccountController(_accountServiceMock.Object);

            // Act
            var result = await controller.ConfirmEmailAsync(_userConfirmationEmailRequest);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, (result as StatusCodeResult).StatusCode);
        }
    }
}
