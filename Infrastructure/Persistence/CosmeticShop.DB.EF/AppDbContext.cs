using CosmeticShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CosmeticShop.DB.EF
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAction> UserActions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product - Category relationship (Many-to-One)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Category - Parent Category (Self-Referencing)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade deletion

            // Product - ProductImage relationship (One-to-Many)
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId);

            // Customer - Favorite relationship (Many-to-Many)
            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.CustomerId, f.ProductId });

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Customer)
                .WithMany(c => c.Favorites)
                .HasForeignKey(f => f.CustomerId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Product)
                .WithMany()
                .HasForeignKey(f => f.ProductId);

            // Customer - Order relationship (One-to-Many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            // Order - OrderItem relationship (One-to-Many)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            // OrderItem - Product relationship (Many-to-One)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // Cart - Customer relationship (One-to-One)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithOne()
                .HasForeignKey<Cart>(c => c.CustomerId);

            // Cart - CartItem relationship (One-to-Many)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId);

            // CartItem - Product relationship (Many-to-One)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);

            // Payment - Order relationship (One-to-One)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne()
                .HasForeignKey<Payment>(p => p.OrderId);

            // Customer - Payment relationship (One-to-Many)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Payments) 
                .HasForeignKey(p => p.CustomerId);

            // Customer - ProductImage relationship (One-to-Many) 
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductImages) 
                .WithOne(pi => pi.Product)     
                .HasForeignKey(pi => pi.ProductId) 
                .OnDelete(DeleteBehavior.Cascade);

            // Product - Review relationship (One-to-Many)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId);

            // Customer - Review relationship (One-to-Many)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId);

            // User - UserAction relationship (One-to-Many)
            modelBuilder.Entity<UserAction>()
                .HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId);
        }
    }

}
