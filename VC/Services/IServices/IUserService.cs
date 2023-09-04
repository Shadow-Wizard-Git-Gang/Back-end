using VC.Models;
using VC.Models.DTOs.UserDTOs;

namespace VC.Services.IServices
{
    public interface IUserService
    {
        public Task<User> CreateUserAsync(UserCreateRequestDTO createRequest);
        public Task<User> GetUserAsync(string id);
        public Task<IEnumerable<User>> GetUsersAsync(int page, int limit);
        public Task<User> UpdateUserAsync(string id, UserUpdateRequestDTO updateRequest);
        public Task DeleteUserAsync(string id);

    }
}
