using System.ComponentModel.DataAnnotations;

namespace VC.Models.DTOs.UserDTOs
{
    public class UserCreateRequestDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string CompanyId { get; set; }
    }
}
