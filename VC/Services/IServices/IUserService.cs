using VC.Models;
using VC.Models.DTOs.UserDTOs;

namespace VC.Services.IServices
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetUsersAsync(int page, int limit);
        public Task<User> GetUserAsync(string id);
        public Task<bool> DeleteUserAsync(string id);
        public Task<User> UpdateUserAsync(string id, UserUpdateRequestDTO user);
        public Task<User> CreateUserAsync(UserCreateRequestDTO user);
    }
}
