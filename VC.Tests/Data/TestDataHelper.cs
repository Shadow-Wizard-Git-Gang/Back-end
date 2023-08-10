using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Driver;
using VC.Models;
using VC.Models.DTOs.AccountDTOs;
using VC.Models.DTOs.UserDTOs;
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

        public static UserCreateRequestDTO CreateUserCreateRequestDTO(
            string? username = null,
            string? email = null,
            string? password = null,
            string? fullName = null,
            int? companyId = null)
        {
            if (companyId != null)
            {
                return new UserCreateRequestDTO { 
                    UserName = username,
                    Email = email,
                    Password = password,
                    FullName = fullName,
                    CompanyId = companyId.Value
                };
            }

            return new UserCreateRequestDTO();
        }

        public static ApplicationUser CreateApplicationUser(
            string? userName = null, 
            string? email = null, 
            List<MongoClaim>? mongoClaims = null)
        {
            if (mongoClaims != null)
            {
                return new ApplicationUser() { 
                    UserName = userName,
                    Email = email, 
                    Claims = mongoClaims
                };
            }

            return new ApplicationUser();
        }

        public static User CreateUser()
        {
            return new User();
        }
    }
}
