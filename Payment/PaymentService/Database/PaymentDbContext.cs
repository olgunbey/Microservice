using Microsoft.EntityFrameworkCore;
using PaymentService.Entities;

namespace PaymentService.Database
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> dbContextOptions) : base(dbContextOptions) { }
        public DbSet<PaymentUser> PaymentUser { get; set; }
    }
}
