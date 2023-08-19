using VC.Models.Identity;

namespace VC.Services.IServices
{
    public interface IEmailService
    {
        public Task SendAsync(string from, string to, string subject, string body);
    }
}
