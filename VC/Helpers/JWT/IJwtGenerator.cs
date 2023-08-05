using VC.Models.Identity;

namespace VC.Helpers.JWT
{
    public interface IJwtGenerator
    {
        public string CreateToken(ApplicationUser user);
    }
}
