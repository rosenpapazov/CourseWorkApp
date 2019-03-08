using CourseWorkApp.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseWorkApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
