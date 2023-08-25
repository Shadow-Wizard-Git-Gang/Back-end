using VC.Models;
using VC.Models.DTOs.AccountDTOs;
using VC.Models.Identity;

namespace VC.Services.IServices
{
    public interface IAccountService
    {
        public Task<UserSignInResponseDTO> SignInAsync(UserSignInRequestDTO userSignInRequest);
        public Task<bool> ConfirmEmailAsync(string userId, string token);
        public Task ResetPassword(string email);
        public Task SetNewPassword(string userId, string token, string password);
    }
}
