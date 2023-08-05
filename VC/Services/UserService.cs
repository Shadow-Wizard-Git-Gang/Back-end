using VC.Models;
using VC.Models.DTOs.UserDTOs;
using VC.Services.IServices;

namespace VC.Services
{
    public class UserService : IUserService
    {
        public UserService(){}

        public Task<User> CreateUserAsync(UserCreateRequestDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<User> EditUserAsync(string id, User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
