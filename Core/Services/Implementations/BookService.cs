using Core.Entities;
using Core.Helpers;
using Core.RepositoriesContracts;
using Core.Services.Contracts;

namespace Core.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ServicesHelpers _servicesHelpers;

        public BookService(IBookRepository bookRepository, ServicesHelpers servicesHelpers)
        {
            _bookRepository = bookRepository;
            _servicesHelpers = servicesHelpers;
        }

        public async Task<List<Book>> GetAllBooksAsync() 
            => await _bookRepository.GetAllBooksAsync();

        public async Task<Book?> GetBookByIdAsync(Guid id) {
            await _servicesHelpers.ThrowIfBookDoesNotExist(id);

            return await _bookRepository.GetBookByIdAsync(id);
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            book.Id = Guid.NewGuid();

            return await _bookRepository.AddBookAsync(book);
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            await _servicesHelpers.ThrowIfBookDoesNotExist(book.Id);

            return await _bookRepository.UpdateBookAsync(book);
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            await _servicesHelpers.ThrowIfBookDoesNotExist(id);

            return await _bookRepository.DeleteBookAsync(id);
        }
    }
}
