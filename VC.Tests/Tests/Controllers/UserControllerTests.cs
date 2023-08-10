﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VC.Controllers;
using VC.Helpers.Exceptions;
using VC.Models;
using VC.Models.DTOs.UserDTOs;
using VC.Services.IServices;
using VC.Tests.Data;
using Xunit;

namespace VC.Tests.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly User _user;
        private readonly UserCreateRequestDTO _userSignUpRequest;
        private readonly Mock<IUserService> _accountServiceMock;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _user = TestDataHelper.CreateUser();
            _userSignUpRequest = new UserCreateRequestDTO();
            _accountServiceMock = new Mock<IUserService>();
            _userController = new UserController(_accountServiceMock.Object);
        }
        [Fact]
        public async Task SignInAsync_Returns201Created_WhenUserCreatedSuccessfully()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.CreateUserAsync(_userSignUpRequest)).ReturnsAsync(_user);

            // Act
            var result = await _userController.SignInAsync(_userSignUpRequest);

            // Assert
            Assert.IsType<CreatedResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, (result as CreatedResult)?.StatusCode);
        }

        [Fact]
        public async Task SignInAsync_Returns400BadRequest_WhenSignUpServiceExceptionThrown()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.CreateUserAsync(_userSignUpRequest)).ThrowsAsync(new SignUpServiceException("Error"));

            // Act
            var result = await _userController.SignInAsync(_userSignUpRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, (result as BadRequestObjectResult)?.StatusCode);
            Assert.Equal("Error", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public async Task SignInAsync_Returns500InternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _accountServiceMock.Setup(x => x.CreateUserAsync(_userSignUpRequest)).ThrowsAsync(new Exception());

            // Act
            var result = await _userController.SignInAsync(_userSignUpRequest);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, (result as StatusCodeResult)?.StatusCode);
        }
    }
}