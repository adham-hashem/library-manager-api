using Core.Entities;

namespace Core.Services.Contracts
{
    public interface IBorrowingRecordService
    {
        Task<IEnumerable<BorrowingRecord>> GetAllBorrowingRecordsAsync();
        Task<BorrowingRecord?> GetBorrowingRecordByIdAsync(Guid id);
        Task<IEnumerable<BorrowingRecord>> GetBorrowingRecordsByUserAsync(Guid userId);
        Task AddBorrowingRecordAsync(BorrowingRecord record);
        Task UpdateBorrowingRecordAsync(BorrowingRecord record);
        Task DeleteBorrowingRecordAsync(Guid id);
        Task<(bool Success, string Message, BorrowingRecord? BorrowingRecord)> BorrowBookAsync(Guid bookId, Guid userId);
        Task<(bool Success, string Message, BorrowingRecord? BorrowingRecord)> ReturnBookAsync(Guid borrowingRecordId);
    }
}
