

namespace Core.DTOs.AccountDtos
{
    public class RegisterRequestDto
    {
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? PostalCode { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
