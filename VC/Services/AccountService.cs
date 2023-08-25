using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Text;
using VC.Helpers.Exceptions;
using VC.Helpers.JWT;
using VC.Models;
using VC.Models.DTOs.AccountDTOs;
using VC.Models.Identity;
using VC.Services.IServices;

namespace VC.Services
{
    public class AccountService : IAccountService
    {
        private SignInManager<ApplicationUser> _signInManager { get; }
        private UserManager<ApplicationUser> _userManager { get; }
        public IEmailService _emailService { get; }
        private IJwtGenerator _jwtGenerator { get; }
        private IMapper _mapper { get; }

        public AccountService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IJwtGenerator jwtGenerator,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;
        }

        public async Task<UserSignInResponseDTO> SignInAsync(UserSignInRequestDTO userSignInRequest)
        {
            var appUser = await _userManager.FindByEmailAsync(userSignInRequest.Email);

            if (appUser == null)
            {
                throw new UnauthorizedException("Invalid Email or Password");
            }

            if (appUser.EmailConfirmed)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(
                    appUser,
                    userSignInRequest.Password,
                    false);

                if (!result.Succeeded)
                {
                    throw new UnauthorizedException("Invalid Email or Password");
                }

                var user = _mapper.Map<User>(appUser);

                user.Password = userSignInRequest.Password;

                var userResponse = new UserSignInResponseDTO
                {
                    User = user,
                    Token = _jwtGenerator.CreateToken(appUser)
                };

                return userResponse;
            }

            throw new UnauthorizedException("Invalid Email or Password");
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException("User not found");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task ResetPassword(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);

            if (appUser == null)
            {
                throw new AppException("The link could not be sent to your email, please try again.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            _emailService.SendPasswordResettingLetterAsync(email, appUser.Id.ToString(), token);
        }

        public async Task SetNewPassword(string userId, string token, string password)
        {
            var appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null)
            {
                throw new UserNotFoundException("User not found");
            }

            var result = await _userManager.ResetPasswordAsync(appUser, token, password);

            if (!result.Succeeded)
            {
                var sb = new StringBuilder();

                foreach (var error in result.Errors)
                {
                    sb.Append(error.Description + "\n");
                }

                throw new AppException(sb.ToString());
            }
        }
    }
}
