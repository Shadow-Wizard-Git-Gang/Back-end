using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace VC.Models.DTOs.CompanyDTOs
{
    public class CompanyUpdateRequestDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public List<string> Employees { get; set; }
    }
}
