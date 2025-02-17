using Core.Enums;

namespace Core.DTOs.AccountDtos
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string? PersonName { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? PostalCode { get; set; }
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public AppRoles AppRole { get; set; } = AppRoles.User;
    }
}
