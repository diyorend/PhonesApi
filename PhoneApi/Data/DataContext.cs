using Microsoft.EntityFrameworkCore;
using PhoneApi.Models;

namespace PhoneApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<PhoneCategory> PhoneCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhoneCategory>()
                .HasKey(pc => new { pc.CategoryId, pc.PhoneId });
            modelBuilder.Entity<PhoneCategory>()
                .HasOne(p => p.Phone)
                .WithMany(pc => pc.PhoneCategories)
                .HasForeignKey(p => p.PhoneId);
            modelBuilder.Entity<PhoneCategory>()
                .HasOne(c => c.Category)
                .WithMany(pc => pc.PhoneCategories)
                .HasForeignKey(c => c.CategoryId);
        }

    }
}
