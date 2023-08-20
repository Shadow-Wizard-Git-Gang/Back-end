using Microsoft.AspNetCore.Mvc;
using VC.Data;
using VC.Models.DTOs.AccountDTOs;
using VC.Services.IServices;

namespace VC.Controllers
{
    [Route(SettingsStorage.RouteNameForAccountController)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService { get; }

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost(SettingsStorage.EndpointNameForSignIn)]    //TODO place in the settings storage
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SignInAsync([FromBody] UserSignInRequestDTO userSignInRequest)
        {
                var response = await _accountService.SignInAsync(userSignInRequest);
                return Ok(response);
        }

        [HttpPost(SettingsStorage.EndpointNameForConfirmEmail)]  //TODO place in the settings storage
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ConfirmEmailAsync([FromBody] UserConfirmationEmailRequest userConfirmationEmailRequest)
        {
            if (await _accountService.ConfirmEmailAsync(
            userConfirmationEmailRequest.UserId,
            userConfirmationEmailRequest.Token))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
