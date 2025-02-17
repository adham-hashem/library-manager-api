namespace Core.DTOs.AccountDtos
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
