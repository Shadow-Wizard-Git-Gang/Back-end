using System.Security.Claims;

namespace VC.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int CompanyId { get; set; }  
        public bool IsOwner { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
