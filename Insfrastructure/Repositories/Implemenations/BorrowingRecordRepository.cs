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

        public async Task<IEnumerable<BorrowingRecord>> GetAllBorrowingRecordsAsync()
            => await _context.BorrowingRecords.Include(br => br.Book).Include(br => br.User).ToListAsync();

        public async Task<BorrowingRecord?> GetBorrowingRecordByIdAsync(Guid id)
            => await _context.BorrowingRecords.Include(br => br.Book).Include(br => br.User).FirstOrDefaultAsync(br => br.Id == id);

        public async Task<IEnumerable<BorrowingRecord>> GetBorrowingRecordsByUserAsync(Guid userId)
            => await _context.BorrowingRecords.Where(br => br.UserId == userId).ToListAsync();
        public async Task AddBorrowingRecordAsync(BorrowingRecord record)
        {
            _context.BorrowingRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBorrowingRecordAsync(BorrowingRecord record)
        {
            _context.BorrowingRecords.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBorrowingRecordAsync(Guid id)
        {
            var record = await _context.BorrowingRecords.FindAsync(id);
            if (record != null)
            {
                _context.BorrowingRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }
    }
}
