﻿using Core.Entities;
using Core.Helpers;
using Core.RepositoriesContracts;
using Core.Services.Contracts;

namespace Core.Services.Implementations
{
    public class BorrowingRecordService : IBorrowingRecordService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowingRecordRepository _borrowingRecordRepository;
        private readonly ServicesHelpers _servicesHelpers;

        public BorrowingRecordService(IBorrowingRecordRepository borrowingRecordRepository,
            IBookRepository bookRepository,
            ServicesHelpers servicesHelpers)
        {
            _borrowingRecordRepository = borrowingRecordRepository;
            _bookRepository = bookRepository;
            _servicesHelpers = servicesHelpers;
        }

        public async Task<List<BorrowingRecord>> GetAllBorrowingRecordsAsync()
            => await _borrowingRecordRepository.GetAllBorrowingRecordsAsync();

        public async Task<BorrowingRecord?> GetBorrowingRecordByIdAsync(Guid id)
        {
            await _servicesHelpers.ThrowIfBorrowingRecordDoesNotExist(id);
            return await _borrowingRecordRepository.GetBorrowingRecordByIdAsync(id);
        }

        public async Task<List<BorrowingRecord>> GetBorrowingRecordsByUserAsync(Guid userId)
        {
            await _servicesHelpers.ThrowIfUserDoesNotExist(userId);
            return await _borrowingRecordRepository.GetBorrowingRecordsByUserAsync(userId);
        }

        public async Task<BorrowingRecord> AddBorrowingRecordAsync(BorrowingRecord record)
            => await _borrowingRecordRepository.AddBorrowingRecordAsync(record);

        public async Task<BorrowingRecord> UpdateBorrowingRecordAsync(BorrowingRecord record)
        {
            await _servicesHelpers.ThrowIfBorrowingRecordDoesNotExist(record.Id);
            return await _borrowingRecordRepository.UpdateBorrowingRecordAsync(record);
        }

        public async Task<bool> DeleteBorrowingRecordAsync(Guid id)
        {
            await _servicesHelpers.ThrowIfBorrowingRecordDoesNotExist(id);
            return await _borrowingRecordRepository.DeleteBorrowingRecordAsync(id);
        }

        public async Task<(bool Success, string Message, BorrowingRecord? BorrowingRecord)> BorrowBookAsync(Guid bookId, Guid userId)
        {
            await _servicesHelpers.ThrowIfUserDoesNotExist(userId);
            await _servicesHelpers.ThrowIfBookDoesNotExist(bookId);

            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null || !book.IsAvailable)
            {
                return (false, "Book is not available", null);
            }

            var borrowingRecord = new BorrowingRecord
            {
                Id = Guid.NewGuid(),
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateTime.UtcNow,
                RentalFee = 10,
                IsPaid = false,
                Book = book
            };

            await _borrowingRecordRepository.AddBorrowingRecordAsync(borrowingRecord);

            return (true, "Book borrowed Successfully", borrowingRecord);
        }

        public async Task<(bool Success, string Message, BorrowingRecord? BorrowingRecord)> ReturnBookAsync(Guid borrowingRecordId)
        {
            {
                var borrowingRecord = await _borrowingRecordRepository.GetBorrowingRecordByIdAsync(borrowingRecordId);
                if (borrowingRecord == null)
                    return (false, "Borrowing record not found", null);

                if (borrowingRecord.ReturnedDate.HasValue)
                    return (false, "Book is already returned", borrowingRecord);

                borrowingRecord.ReturnedDate = DateTime.UtcNow;
                borrowingRecord.CalculateFine();

                await _borrowingRecordRepository.UpdateBorrowingRecordAsync(borrowingRecord);

                return (true, "Book returned Successfully", borrowingRecord);
            }
        }
    }
}
