using System.Security.Claims;

namespace VC.Models
{
    public class User
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int CompanyId { get; set; }  
        public bool IsOwner { get; set; }
        public List<Claim> Claims { get; set; } = null!;
    }
}
