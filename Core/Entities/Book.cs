using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Book
    {
        [Required]
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public bool IsAvailable { get; set; }

        // Fine per day for Late return
        public decimal? LateReturnFinePerDay { get; set; } = 0;

        public ICollection<BorrowingRecord> BorrowingRecords { get; set; } = new List<BorrowingRecord>();
    }
}
