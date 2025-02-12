using Core.Entities;

namespace Core.RepositoriesContracts
{
    public interface IBorrowingRecordRepository
    {
        Task<List<BorrowingRecord>> GetAllBorrowingRecordsAsync();
        Task<BorrowingRecord?> GetBorrowingRecordByIdAsync(Guid id);
        Task<List<BorrowingRecord>> GetBorrowingRecordsByUserAsync(Guid userId);
        Task<BorrowingRecord> AddBorrowingRecordAsync(BorrowingRecord record);
        Task<BorrowingRecord> UpdateBorrowingRecordAsync(BorrowingRecord record);
        Task<bool> DeleteBorrowingRecordAsync(Guid id);
    }
}
