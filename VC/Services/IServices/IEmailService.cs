using VC.Models.Identity;

namespace VC.Services.IServices
{
    public interface IEmailService
    {
        public Task SendConfirmationLetterAsync(string id, string email, string confirmationToken);
    }
}
