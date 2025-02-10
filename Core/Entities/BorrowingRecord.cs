using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class BorrowingRecord
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BookId { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        // Payment Details
        public decimal RentalFee { get; set; } = 0; // Borrowing cost
        public decimal? FineAmount { get; set; } = 0; // Late fine (auto-calculated)
        public bool IsPaid { get; set; } = false;

        // Navigation Properties
        public Book Book { get; set; }
        public ApplicationUser User { get; set; }

        // Calculate fine amount automatically when book is returned
        public void CalculateFine()
        {
            if (ReturnedDate.HasValue)
            {
                var dueDate = BorrowDate.AddDays(14);
                if (ReturnedDate > dueDate)
                {
                    var lateDays = (ReturnedDate.Value - dueDate).Days;
                    FineAmount = lateDays * Book.LateReturnFinePerDay;
                }
                else
                {
                    FineAmount = 0;
                }
            }
        }
    }
}
