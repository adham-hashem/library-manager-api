using Core.Entities;

namespace Core.Services.Contracts
{
    public interface IBorrowingRecordService
    {
        Task<List<BorrowingRecord>> GetAllBorrowingRecordsAsync();
        Task<BorrowingRecord?> GetBorrowingRecordByIdAsync(Guid id);
        Task<List<BorrowingRecord>> GetBorrowingRecordsByUserAsync(Guid userId);
        Task<BorrowingRecord> AddBorrowingRecordAsync(BorrowingRecord record);
        Task<BorrowingRecord> UpdateBorrowingRecordAsync(BorrowingRecord record);
        Task<bool> DeleteBorrowingRecordAsync(Guid id);
        Task<(bool Success, string Message, BorrowingRecord? BorrowingRecord)> BorrowBookAsync(Guid bookId, Guid userId);
        Task<(bool Success, string Message, BorrowingRecord? BorrowingRecord)> ReturnBookAsync(Guid borrowingRecordId);
    }
}
