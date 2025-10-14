using Dicount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Dicount.Grpc.Data
{
    public class DiscountContext : DbContext
    {
        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon
                {
                    Id = 1,
                    ProductName = "IPhone X",
                    Amount = 150,
                    Description = "IPhone Discount"
                },
                new Coupon
                {
                    Id = 2,
                    Description = "Samsung 10 Discount",
                    Amount = 100,
                    ProductName = "Samsung 10"
                });
        }

        public DbSet<Coupon> Coupons { get; set; } = default!;
    }
}
