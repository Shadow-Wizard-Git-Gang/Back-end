using System.ComponentModel.DataAnnotations;

namespace VC.Models.DTOs.AccountDTOs
{
    public class UserConfirmationEmailRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
