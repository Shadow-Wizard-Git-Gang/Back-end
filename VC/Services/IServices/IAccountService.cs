using VC.Models;
using VC.Models.DTOs.AccountDTOs;
using VC.Models.Identity;

namespace VC.Services.IServices
{
    public interface IAccountService
    {
        public Task<UserSignInResponseDTO> SignInAsync(UserSignInRequestDTO userSignInRequest);
        public Task<bool> ConfirmEmailAsync(string userId, string token);
    }
}
