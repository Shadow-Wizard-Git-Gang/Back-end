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
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return false;
            return true;
        }

        public async Task<User> UpdateUserAsync(string id, UserUpdateRequestDTO userRequest)
        {
            var appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return null;
            }

            _mapper.Map(userRequest, appUser);

            var result = await _userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
            {
                return null;
            }

            var user = _mapper.Map<User>(appUser);

            return user;
        }

        public async Task<User> GetUserAsync(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null) { return null; }
            var user = _mapper.Map<User>(appUser);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(int page, int limit)
        {
            int startIndex = (page - 1) * limit;
            int endIndex = startIndex + limit;

            var appUsers = _userManager.Users
                .Skip(startIndex)
                .Take(endIndex)
                .ToList();

            var users = _mapper.Map<List<User>>(appUsers);

            return users;
        }
    }
}
