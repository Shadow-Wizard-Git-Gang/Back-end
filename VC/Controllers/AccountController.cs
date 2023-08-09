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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SignInAsync([FromBody] UserSignInRequestDTO userSignInRequest)
        {
            try
            {
                var response = await _accountService.SignInAsync(userSignInRequest);

                if (response == null)
                {
                    return Unauthorized("Invalid Email or Password");
                }

                return Ok(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
