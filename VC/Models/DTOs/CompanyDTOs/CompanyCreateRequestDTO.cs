using System.ComponentModel.DataAnnotations;

namespace VC.Models.DTOs.CompanyDTOs
{
    public class CompanyCreateRequestDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
