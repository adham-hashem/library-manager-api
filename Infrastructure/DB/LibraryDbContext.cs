using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB
{
    public class LibraryDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowingRecord> BorrowingRecords { get; set; }
        public DbSet<ExpiredToken> ExpiredTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BorrowingRecord>()
                .HasKey(br => br.Id); // Primary Key

            builder.Entity<BorrowingRecord>()
                .HasOne(br => br.User)
                .WithMany(u => u.BorrowingRecords)
                .HasForeignKey(br => br.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BorrowingRecord>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowingRecords)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
