using Core.Entities;
using Infrastructure.DB;
using Core.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemenations
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBooksAsync() => await _context.Books.ToListAsync();

        public async Task<Book?> GetBookByIdAsync(Guid id) {
            return await _context.Books
                .AsNoTracking() // Prevent EF from tracking the entity
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return false;
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

