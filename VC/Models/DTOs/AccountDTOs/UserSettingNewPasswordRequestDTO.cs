using System.ComponentModel.DataAnnotations;

namespace VC.Models.DTOs.AccountDTOs
{
    public class UserSettingNewPasswordRequestDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
