using Microsoft.EntityFrameworkCore;
using eshop.api.Entities;

namespace eshop.api.Data;

public class DataContext(DbContextOptions options) : DbContext(options)

{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasKey(c => c.OrderId);
        modelBuilder.Entity<Customer>().HasKey(x => x.CustomerId);
        modelBuilder.Entity<OrderItem>().HasKey(c => new { c.OrderId, c.ProductId });

        base.OnModelCreating(modelBuilder);
    }

}