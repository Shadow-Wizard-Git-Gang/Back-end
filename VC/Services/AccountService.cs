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
        public SignInManager<ApplicationUser> _signInManager { get; }
        private UserManager<ApplicationUser> _userManager { get; }
        private IJwtGenerator _jwtGenerator { get; }
        public IMapper _mapper { get; }

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
                return null;
            }

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
    }

}
