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
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserAsync(string id)
        {
            try
            {
                var response = await _userService.GetUserAsync(id);
                if (response == null) return NotFound();
                return Ok(response);
            }
            catch (FormatException)
            {
                return BadRequest("Wrong id format");
            }
            catch (IndexOutOfRangeException)
            {
                return BadRequest("Wrong id format");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                var response = await _userService.DeleteUserAsync(id);
                if (!response) return NotFound();
                return NoContent();
            }
            catch (FormatException)
            {
                return BadRequest();
            }
            catch (IndexOutOfRangeException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }

}
