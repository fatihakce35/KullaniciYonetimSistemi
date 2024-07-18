using Microsoft.EntityFrameworkCore;
using KayitSistemiApi.Models;

namespace KayitSistemiApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        }

        }
}
