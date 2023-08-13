using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Text;
using VC.Helpers.Exceptions;
using VC.Helpers.JWT;
using VC.Models;
using VC.Models.DTOs.UserDTOs;
using VC.Models.Identity;
using VC.Services.IServices;

namespace VC.Services
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager { get; }
        public IMapper _mapper { get; }

        public UserService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<User> CreateUserAsync(UserCreateRequestDTO user)
        {
            var appUser = _mapper.Map<ApplicationUser>(user);

            var result = await _userManager.CreateAsync(appUser, user.Password);

            if (!result.Succeeded)
            {
                var sb = new StringBuilder();

                foreach (var error in result.Errors)
                {
                    sb.Append(error.Description + "\n");
                }

                throw new SignUpServiceException(sb.ToString());
            }

            return _mapper.Map<User>(appUser);
        }

        public async Task<bool> DeleteUserAsync(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) { return false; }
            var appUser = _mapper.Map<User>(user);
            await _userManager.DeleteAsync(user);
            return true;
        }

        public Task<User> EditUserAsync(string id, User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var appUser = _mapper.Map<User>(user);
            return appUser;
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
