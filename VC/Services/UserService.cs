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
        private IAccountService _accountService { get; }
        private IMapper _mapper { get; }

        public UserService(
            UserManager<ApplicationUser> userManager,
            IAccountService accountService,
            IMapper mapper)
        {
            _userManager = userManager;
            _accountService = accountService;
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

            try
            {
                await _accountService.SendConfirmationLetterAsync(appUser);
            }
            catch
            {
                await _userManager.DeleteAsync(appUser);

                throw new SignUpServiceException("Problem with sending confirmation letter");
            }

            return _mapper.Map<User>(appUser);
        }

        public Task<bool> DeleteUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<User> EditUserAsync(string id, User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
