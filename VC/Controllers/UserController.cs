using Microsoft.AspNetCore.Mvc;
using VC.Data;
using VC.Helpers.Exceptions;
using VC.Models.DTOs.AccountDTOs;
using VC.Models.DTOs.UserDTOs;
using VC.Services.IServices;

namespace VC.Controllers
{
    [Route(SettingsStorage.RouteNameForUserController)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService { get; }

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateUserAsync([FromBody] UserCreateRequestDTO userSignUpRequest)
        {
            try
            {
                var response = await _userService.CreateUserAsync(userSignUpRequest);

                return Created("", response);//TODO make URI
            }
            catch (SignUpServiceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

}
