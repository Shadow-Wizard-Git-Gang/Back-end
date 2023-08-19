using System.ComponentModel.DataAnnotations;

namespace VC.Models.DTOs.AccountDTOs
{
    public class UserConfirmationEmailRequest
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;
    }
}
