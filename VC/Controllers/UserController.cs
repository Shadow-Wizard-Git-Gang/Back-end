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
            var response = await _userService.CreateUserAsync(userSignUpRequest);
            return Created("", response);//TODO make URI
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserAsync(string id)
        {
            var response = await _userService.GetUserAsync(id);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUsersAsync(int page, int limit)
        {
            if (page <= 0 || limit <= 0)
            {
                return BadRequest("Page and limit must be greater than zero.");
            }

            var result = await _userService.GetUsersAsync(page, limit);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserAsync(string id, [FromBody] UserUpdateRequestDTO userUpdateRequest)
        {
            var result = await _userService.UpdateUserAsync(id, userUpdateRequest);
            return Ok(result);
        }
    }
}
