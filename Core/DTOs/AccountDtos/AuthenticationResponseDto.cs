namespace Core.DTOs.AccountDtos
{
    public class AuthenticationResponseDto
    {
        public string? Token { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? PersonName { get; set; }
    }
}
