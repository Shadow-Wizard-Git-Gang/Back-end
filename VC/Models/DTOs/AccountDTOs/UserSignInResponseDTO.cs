namespace VC.Models.DTOs.AccountDTOs
{
    public class UserSignInResponseDTO
    {
        public User User { get; set; } = null!;

        public string Token { get; set; } = null!;
    }
}
