using VC.Models.DTOs.AccountDTOs;

namespace VC.Services.IServices
{
    public interface IAccountService
    {
        public Task<UserSignInResponseDTO?> SignInAsync(UserSignInRequestDTO userSignInRequest);
    }
}
