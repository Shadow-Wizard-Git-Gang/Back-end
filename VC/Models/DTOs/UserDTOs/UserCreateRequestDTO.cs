using System.ComponentModel.DataAnnotations;

namespace VC.Models.DTOs.UserDTOs
{
    public class UserCreateRequestDTO
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public int CompanyId { get; set; }
    }
}
