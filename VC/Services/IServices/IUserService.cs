using VC.Models;
using VC.Models.DTOs.UserDTOs;

namespace VC.Services.IServices
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<User> GetUserAsync(string id);
        public Task<bool> DeleteUserAsync(string id);
        public Task<User> EditUserAsync(string id, User user);
        public Task<User> CreateUserAsync(UserCreateRequestDTO user);
    }
}
