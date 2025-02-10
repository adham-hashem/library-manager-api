using Core.Entities;
using Core.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Get all books.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
            {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        /// <summary>
        /// Get a single book by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}", Name = "GetBookById")]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return NotFound(new { message = "Book not found" });

            return Ok(book);
        }

        /// <summary>
        /// Add a new book.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            if (book == null)
                return BadRequest(new { message = "Invalid book data" });

            book.Id = Guid.NewGuid();
            await _bookService.AddBookAsync(book);
            return CreatedAtAction("GetBookById", new { id = book.Id }, book);
        }


        /// <summary>
        /// Update an existing book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedBook"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] Book updatedBook)
        {
            if (updatedBook == null || id != updatedBook.Id)
                return BadRequest(new { message = "Invalid book data or ID mismatch" });

            var existingBook = await _bookService.GetBookByIdAsync(id);
            if (existingBook == null)
                return NotFound(new { message = "Book not found" });

            await _bookService.UpdateBookAsync(updatedBook);
            return Ok(new { message = "Book updated successfully" });
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var exsitingBook = await _bookService.GetBookByIdAsync(id);
            if (exsitingBook == null)
                return NotFound(new { message = "Book not found." });

            await _bookService.DeleteBookAsync(id);
            return Ok(new { message = "Book deleted successfully." });
        }
    }
}