using Core.Entities;
using Core.RepositoriesContracts;
using Microsoft.AspNetCore.Identity;

namespace Core.Helpers
{
    public class ServicesHelpers
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowingRecordRepository _borrowingRecordRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServicesHelpers(IBookRepository bookRepository, IBorrowingRecordRepository borrowingRecordRepository, UserManager<ApplicationUser> userManager)
        {
            _bookRepository = bookRepository;
            _borrowingRecordRepository = borrowingRecordRepository;
            _userManager = userManager;
        }

        public async Task ThrowIfBookDoesNotExist(Guid bookId)
        {
            Book? book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null)
                throw new Exception("Book does not exist!");
        }

        public async Task ThrowIfBorrowingRecordDoesNotExist(Guid borrowingRecordId)
        {
            BorrowingRecord? borrowingRecord = await _borrowingRecordRepository.GetBorrowingRecordByIdAsync(borrowingRecordId);
            if (borrowingRecord == null)
                throw new Exception("Borrowing record does not exist!");
        }

        public async Task ThrowIfUserDoesNotExist(Guid userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception("User does not exist!");
        }
    }
}
