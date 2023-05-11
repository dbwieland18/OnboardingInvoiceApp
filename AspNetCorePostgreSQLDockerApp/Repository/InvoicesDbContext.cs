using Microsoft.EntityFrameworkCore;
using AspNetCorePostgreSQLDockerApp.Models;

namespace AspNetCorePostgreSQLDockerApp.Repository
{
    public class InvoicesDbContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public InvoicesDbContext (DbContextOptions<InvoicesDbContext> options) : base(options) { }
    }
}