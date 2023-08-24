using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private IJwtGenerator _jwtGenerator { get; }
        private IMapper _mapper { get; }

        public AccountService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IJwtGenerator jwtGenerator,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
    }
}
