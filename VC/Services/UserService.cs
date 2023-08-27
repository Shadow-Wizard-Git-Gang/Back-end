using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Text;
using VC.Helpers.Exceptions;
using VC.Models;
using VC.Models.DTOs.UserDTOs;
using VC.Models.Identity;
using VC.Services.IServices;

namespace VC.Services
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager { get; }
        public IEmailService _emailService { get; }
        private IMapper _mapper { get; }

        public UserService(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IMapper mapper)
        {
            _userManager = userManager;
            _emailService = emailService;
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

                throw new AppException(sb.ToString());
            }

            _emailService.SendConfirmationLetterAsync(
                appUser.Email,
                appUser.Id.ToString(),
                await _userManager.GenerateEmailConfirmationTokenAsync(appUser));

            return _mapper.Map<User>(appUser);
        }

        public async Task DeleteUserAsync(string id)
        {
            var appUser = await GetAppUserAsync(id);

            await _userManager.DeleteAsync(appUser);
        }

        public async Task<User> UpdateUserAsync(string id, UserUpdateRequestDTO userRequest)
        {
            var appUser = await GetAppUserAsync(id);

            _mapper.Map(userRequest, appUser);

            var result = await _userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
            {
                throw new AppException();
            }

            return _mapper.Map<User>(appUser);
        }

        public async Task<User> GetUserAsync(string id)
        {
            var appUser = await GetAppUserAsync(id);

            return _mapper.Map<User>(appUser);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(int page, int limit)
        {
            int startIndex = (page - 1) * limit;
            int endIndex = startIndex + limit;

            var appUsers = _userManager.Users
                .Skip(startIndex)
                .Take(endIndex)
                .ToList();

            return _mapper.Map<List<User>>(appUsers);
        }

        // helper methods

        private async Task<ApplicationUser> GetAppUserAsync(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                throw new UserNotFoundException("User not found");
            }

            return appUser;
        }
    }
}
