using Core.Entities;
using Core.RepositoriesContracts;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemenations
{
    public class BorrowingRecordRepository : IBorrowingRecordRepository
    {
        private readonly LibraryDbContext _context;

        public BorrowingRecordRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<BorrowingRecord>> GetAllBorrowingRecordsAsync()
            => await _context.BorrowingRecords.ToListAsync();
        //=> await _context.BorrowingRecords.Include(br => br.Book).Include(br => br.User).ToListAsync();


        public async Task<BorrowingRecord?> GetBorrowingRecordByIdAsync(Guid id)
            => await _context.BorrowingRecords.FirstOrDefaultAsync(br => br.Id == id);
        //=> await _context.BorrowingRecords.Include(br => br.Book).Include(br => br.User).FirstOrDefaultAsync(br => br.Id == id);

        public async Task<List<BorrowingRecord>> GetBorrowingRecordsByUserAsync(Guid userId)
            => await _context.BorrowingRecords.Where(br => br.UserId == userId).ToListAsync();
        public async Task<BorrowingRecord> AddBorrowingRecordAsync(BorrowingRecord record)
        {
            _context.BorrowingRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<BorrowingRecord> UpdateBorrowingRecordAsync(BorrowingRecord record)
        {
            _context.BorrowingRecords.Update(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<bool> DeleteBorrowingRecordAsync(Guid id)
        {
            var record = await _context.BorrowingRecords.FindAsync(id);
            if (record == null) return false;
            _context.BorrowingRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
