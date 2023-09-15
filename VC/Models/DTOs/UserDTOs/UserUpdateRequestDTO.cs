using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace VC.Models.DTOs.UserDTOs
{
    public class UserUpdateRequestDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string CompanyId { get; set; }

        [Required]
        public bool IsOwner { get; set; }

        [Required]
        public List<Claim> Claims { get; set; }
    }
}
