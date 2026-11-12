using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des relations
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Article)
                .WithMany(a => a.OrderItems)
                .HasForeignKey(oi => oi.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Article)
                .WithMany(a => a.CartItems)
                .HasForeignKey(ci => ci.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Données initiales
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Entrées", Description = "Plats pour commencer le repas" },
                new Category { Id = 2, Name = "Plats Principaux", Description = "Plats principaux du restaurant" },
                new Category { Id = 3, Name = "Desserts", Description = "Douceurs pour terminer le repas" },
                new Category { Id = 4, Name = "Boissons", Description = "Boissons fraîches et chaudes" }
            );

            modelBuilder.Entity<Article>().HasData(
                new Article
                {
                    Id = 1,
                    Name = "Salade César",
                    Description = "Salade fraîche avec poulet grillé et croutons",
                    Price = 12.50m,
                    CategoryId = 1,
                    ImageUrl = "/images/salade-cesar.jpg",
                    IsAvailable = true
                },
                new Article
                {
                    Id = 2,
                    Name = "Steak Frites",
                    Description = "Steak de bœuf 200g avec frites maison",
                    Price = 18.90m,
                    CategoryId = 2,
                    ImageUrl = "/images/steak-frites.jpg",
                    IsAvailable = true
                }
            );
        }
    }
}