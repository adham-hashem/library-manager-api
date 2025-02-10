namespace Core.RepositoriesContracts
{
    public interface ITokenRepository
    {
        Task AddExpiredToken(string token);

        bool IsExpiredToken(string token);
    }
}
