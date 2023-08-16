using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private IConfiguration _configuration { get; }
        private IJwtGenerator _jwtGenerator { get; }
        private IEmailService _emailService { get; }
        private IMapper _mapper { get; }

        public AccountService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IJwtGenerator jwtGenerator,
            IEmailService emailService,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _jwtGenerator = jwtGenerator;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<UserSignInResponseDTO?> SignInAsync(UserSignInRequestDTO userSignInRequest)
        {
            var appUser = await _userManager.FindByEmailAsync(userSignInRequest.Email);

            if (appUser == null)
            {
                return null;
            }

            if (appUser.EmailConfirmed)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(
                    appUser,
                    userSignInRequest.Password,
                    false);

                if (!result.Succeeded)
                {
                    return null;
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

            return null;
        }

        public async Task SendConfirmationLetterAsync(ApplicationUser applicationUser)
        {
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

            var confirmationLink = string.Format(
                _configuration["URLToTheConfirmationPage"],
                applicationUser.Id,
                confirmationToken);

            await _emailService.SendAsync(
                _configuration["OrganizationEmail"],
                applicationUser.Email,
                "Please confirm your email",
                $"Please click on this link to confirm your email address: {confirmationLink}");
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}
