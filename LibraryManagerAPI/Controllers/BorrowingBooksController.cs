using Core.Entities;
using Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingBooksController : ControllerBase
    {
        private readonly IBorrowingRecordService _borrowingRecordService;
        public BorrowingBooksController(IBorrowingRecordService borrowingRecordService)
        {
            _borrowingRecordService = borrowingRecordService;
        }

        /// <summary>
        /// Borrow a book.
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>

        [HttpPost("brrow/{bookId:guid}")]
        public async Task<IActionResult> BorrowBook(Guid bookId)
        {
            var userId = User.Identity?.Name;
            if (String.IsNullOrEmpty(userId)) {
                return Unauthorized(new { message = "User is not authorized" });
            };

            var result = await _borrowingRecordService.BorrowBookAsync(bookId, Guid.Parse(userId));
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = "Book borrowed successfully", data = result.BorrowingRecord });
        }

        /// <summary>
        /// Return a borrowed book.
        /// </summary>
        /// <param name="borrowingRecordId"></param>
        /// <returns></returns>

        [HttpPost("return/{borrowingRecordId:guid}")]
        public async Task<IActionResult> ReturnBook(Guid borrowingRecordId)
        {
            var result = await _borrowingRecordService.ReturnBookAsync(borrowingRecordId);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Book returned successfully", data = result.BorrowingRecord });
        }

        /// <summary>
        /// Get all borrowing records for the logged-in user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("my-borrowings")]
        public async Task<IActionResult> GetMyBorrowingRecords()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User is not authorized" });

            var records = await _borrowingRecordService.GetBorrowingRecordByIdAsync(Guid.Parse(userId));
            return Ok(records);
        }

        /// <summary>
        /// Get all borrowing records (admin only).
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBorrowingRecords()
        {
            var records = await _borrowingRecordService.GetAllBorrowingRecordsAsync();
            return Ok(records);
        }
    }
}
