using VC.Models.DTOs.AccountDTOs;
using VC.Services.IServices;

namespace VC.Services
{
    public class AccountService : IAccountService
    {
        public AccountService(){}

        public Task<UserSignInResponseDTO> SignInAsync(UserSignInRequestDTO userSignInRequest)
        {
            throw new NotImplementedException();
        }
    }
}
