using Core.DTOs.AccountDtos;
using Core.Entities;
using Core.RepositoriesContracts;
using Core.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Services.Implementations
{
    public class JwtServices : IJwtServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public JwtServices(UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        public async Task AddExpiredToken(string token)
        {
            await _tokenRepository.AddExpiredToken(token);
        }

        public async Task<AuthenticationResponseDto> CreateJwtToken(ApplicationUser user)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(Environment.GetEnvironmentVariable("LIBRARYMANAGER_DEV_JWT_EXPIRATION_MINUTES")));

            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("LIBRARYMANAGER_DEV_JWT_SECRET")!));

            SigningCredentials signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
                Environment.GetEnvironmentVariable("LIBRARYMANAGER_DEV_JWT_ISSUER"),
                null,
                claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenGenerator);

            return new AuthenticationResponseDto
            {
                Token = token,
                ExpirationDate = expiration,
                UserId = user.Id.ToString(),
                Email = user.Email,
                PersonName = user.PersonName,
            };
        }

        public bool IsExpiredToken(string token)
        {
            return _tokenRepository.IsExpiredToken(token);
        }
    }
}
