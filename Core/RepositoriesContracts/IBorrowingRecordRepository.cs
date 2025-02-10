using Core.Entities;

namespace Core.RepositoriesContracts
{
    public interface IBorrowingRecordRepository
    {
        Task<IEnumerable<BorrowingRecord>> GetAllBorrowingRecordsAsync();
        Task<BorrowingRecord?> GetBorrowingRecordByIdAsync(Guid id);
        Task<IEnumerable<BorrowingRecord>> GetBorrowingRecordsByUserAsync(Guid userId);
        Task AddBorrowingRecordAsync(BorrowingRecord record);
        Task UpdateBorrowingRecordAsync(BorrowingRecord record);
        Task DeleteBorrowingRecordAsync(Guid id);
    }
}
