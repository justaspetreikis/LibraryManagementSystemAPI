using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Database
{
    public class LibraryManagementDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProfileImage> Images { get; set; }
        public LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.PersonalCode)
            .IsUnique();
        }
    }
}
