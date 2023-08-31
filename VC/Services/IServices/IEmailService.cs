using VC.Models.Identity;

namespace VC.Services.IServices
{
    public interface IEmailService
    {
        public Task SendConfirmationLetterAsync(string email, string id, string confirmationToken);
        public Task SendPasswordResettingLetterAsync(string email, string id, string resettingToken);
    }
}
