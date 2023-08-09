using VC.Models;
using VC.Models.DTOs.AccountDTOs;
using VC.Models.Identity;

namespace VC.Tests.Data
{
    public static class TestDataHelper
    {
        public static UserSignInRequestDTO CreateUserSignInRequestDTO(string? email = null, string? password = null)
        {
            if (email != null && password != null)
            {
                return new UserSignInRequestDTO { 
                    Email = email, 
                    Password = password
                };
            }

            return new UserSignInRequestDTO {
                Email = "test@test.com",
                Password = "password"
            };
        }

        public static UserSignInResponseDTO CreateUserSignInResponseDTO(User? user = null, string? token = null)
        {
            if (user != null && token != null)
            {
                return new UserSignInResponseDTO { 
                    User = user, 
                    Token = token 
                };
            }

            return new UserSignInResponseDTO();
        }

        public static ApplicationUser CreateApplicationUser()
        {
            return new ApplicationUser();
        }

        public static User CreateUser()
        {
            return new User();
        }
    }
}
