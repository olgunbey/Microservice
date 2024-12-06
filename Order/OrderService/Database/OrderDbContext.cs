using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.Database
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> dbContextOptions) : base(dbContextOptions) { }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }

    }
}
