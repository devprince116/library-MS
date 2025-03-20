using Authentication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
        public DbSet<Books> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        
    }
}