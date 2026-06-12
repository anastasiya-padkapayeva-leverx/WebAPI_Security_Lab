using SecurityLab.Api.Common;
using SecurityLab.Api.Contracts;
using SecurityLab.Api.Models;

namespace SecurityLab.Api.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db, IPasswordHasher hasher)
    {
        db.Database.EnsureCreated();
        if (db.Users.Any()) return;

        var admin = new User { Email = "admin@lab.local", PasswordHash = hasher.Hash("Admin123!"), Role = Roles.Admin };
        var alice = new User { Email = "alice@lab.local", PasswordHash = hasher.Hash("Password1!"), Role = Roles.User };
        var bob   = new User { Email = "bob@lab.local",   PasswordHash = hasher.Hash("Password1!"), Role = Roles.User };
        db.Users.AddRange(admin, alice, bob);

        var now = DateTimeOffset.UtcNow;
        db.Products.AddRange(
            new Product { Name = "Wireless Mouse",              Category = "Accessories", Price = 25.50m,  Rating = 4.3, Stock = 120, CreatedAt = now.AddDays(-40) },
            new Product { Name = "Mechanical Keyboard",         Category = "Accessories", Price = 89.00m,  Rating = 4.7, Stock = 60,  CreatedAt = now.AddDays(-30) },
            new Product { Name = "27\" Monitor",                Category = "Displays",    Price = 199.99m, Rating = 4.5, Stock = 35,  CreatedAt = now.AddDays(-25) },
            new Product { Name = "USB-C Hub",                   Category = "Accessories", Price = 39.90m,  Rating = 4.1, Stock = 200, CreatedAt = now.AddDays(-20) },
            new Product { Name = "Laptop Stand",                Category = "Accessories", Price = 29.00m,  Rating = 4.0, Stock = 150, CreatedAt = now.AddDays(-15) },
            new Product { Name = "Noise-Cancelling Headphones", Category = "Audio",       Price = 149.00m, Rating = 4.6, Stock = 45,  CreatedAt = now.AddDays(-10) }
        );
        db.SaveChanges();

        db.Orders.AddRange(
            new Order
            {
                UserId = alice.Id, Status = OrderStatus.Paid, Total = 114.50m, CreatedAt = now.AddDays(-5),
                Items =
                {
                    new OrderItem { ProductName = "Wireless Mouse",      Quantity = 1, UnitPrice = 25.50m },
                    new OrderItem { ProductName = "Mechanical Keyboard", Quantity = 1, UnitPrice = 89.00m }
                }
            },
            new Order
            {
                UserId = bob.Id, Status = OrderStatus.Pending, Total = 199.99m, CreatedAt = now.AddDays(-2),
                Items =
                {
                    new OrderItem { ProductName = "27\" Monitor", Quantity = 1, UnitPrice = 199.99m }
                }
            }
        );
        db.SaveChanges();
    }
}
