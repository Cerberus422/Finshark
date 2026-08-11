using api.Mappers;
using Microsoft.EntityFrameworkCore;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Stock)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Portfolio>(p => p.HasKey(p => new { p.AppUserId, p.StockId }));

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);
                
            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);
            
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "Admin",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "Admin",
                },

                new IdentityRole
                {
                    Id = "User",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "User",
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}