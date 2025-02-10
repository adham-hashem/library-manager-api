using Core.Entities;
using Core.RepositoriesContracts;
using Core.Services.Contracts;

namespace Core.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync() => await _bookRepository.GetAllBooksAsync();

        public async Task<Book?> GetBookByIdAsync(Guid id) => await _bookRepository.GetBookByIdAsync(id); 

        public async Task AddBookAsync(Book book) => await _bookRepository.AddBookAsync(book);

        public async Task UpdateBookAsync(Book book) => await _bookRepository.UpdateBookAsync(book);

        public async Task DeleteBookAsync(Guid id) => await _bookRepository.DeleteBookAsync(id);
    }
}
