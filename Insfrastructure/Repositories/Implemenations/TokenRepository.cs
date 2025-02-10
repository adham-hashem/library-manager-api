using Core.Entities;
using Infrastructure.DB;

namespace Infrastructure.Repositories.Implemenations
{
    public class TokenRepository
    {
        private readonly LibraryDbContext _context;
        public TokenRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task AddExpiredToken(string token)
        {
            ExpiredToken expiredToken = new ExpiredToken
            {
                Id = Guid.NewGuid(),
                Token = token
            };
            await _context.ExpiredTokens.AddAsync(expiredToken);
            await _context.SaveChangesAsync();
        }

        public bool IsExpiredToken(string token)
        {
            return _context.ExpiredTokens.Any(t => t.Token == token);
        }
    }
}
