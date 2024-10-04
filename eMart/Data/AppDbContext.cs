using eMart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace eMart.Data
{
    public class AppDbContext :IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Product> products { get; set; }

        public DbSet<Brand> brands { get; set; }
      
        public DbSet<Category> categories { get; set; }

        public DbSet<SubCategory> subCategories {  get; set; }  
        
        public DbSet<Cart> carts { get; set; }

        public DbSet<CartProducts> cartProducts { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<OrderProducts> OrderProducts { get; set; }

        public DbSet<Review> Reviews  { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
                );
            base.OnModelCreating(modelBuilder);
        }


    }
}
