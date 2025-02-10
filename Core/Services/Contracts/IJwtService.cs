using Core.DTOs.AccountDtos;
using Core.Entities;

namespace Core.Services.Contracts
{
    public interface IJwtService
    {
        Task<AuthenticationResponseDto> CreateJwtToken(ApplicationUser user);
    }
}
