using System.ComponentModel.DataAnnotations;

namespace VC.Models.DTOs.AccountDTOs
{
    public class UserSignInRequestDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
