using LearnApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Contact>(e => 
            {
                e.Property(c => c.FirstName).HasMaxLength(100).IsRequired();
                e.Property(c => c.LastName).HasMaxLength(100).IsRequired();
                e.Property(c => c.Email).HasMaxLength(256).IsRequired();
                e.HasIndex(c => c.Email).IsUnique();

                e.HasMany(c => c.Addresses)
                 .WithOne(a => a.Contact)
                 .HasForeignKey(a => a.ContactId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Address>(e =>
            {
                e.Property(a => a.Street).HasMaxLength(200);
                e.Property(a => a.City).HasMaxLength(100);
                e.Property(a => a.State).HasMaxLength(2);
                e.Property(a => a.PostalCode).HasMaxLength(10);
            });

            // Seed data, inserted by the migration. Sofia has no addresses on purpose.
            modelBuilder.Entity<Contact>().HasData(
                new Contact { Id = 1, FirstName = "Maria", LastName = "Lopez", Email = "maria.lopez@example.com" },
                new Contact { Id = 2, FirstName = "James", LastName = "Carter", Email = "james.carter@example.com" },
                new Contact { Id = 3, FirstName = "Aisha", LastName = "Khan", Email = "aisha.khan@example.com" },
                new Contact { Id = 4, FirstName = "Daniel", LastName = "Kim", Email = "daniel.kim@example.com" },
                new Contact { Id = 5, FirstName = "Sofia", LastName = "Rossi", Email = "sofia.rossi@example.com" });

            modelBuilder.Entity<Address>().HasData(
                new Address { Id = 1, ContactId = 1, Street = "12 Oak St", City = "Springfield", State = "IL", PostalCode = "62701" },
                new Address { Id = 2, ContactId = 1, Street = "400 Lake Ave", City = "Madison", State = "WI", PostalCode = "53703" },
                new Address { Id = 3, ContactId = 2, Street = "9 Pine Rd", City = "Fairview", State = "TN", PostalCode = "37062" },
                new Address { Id = 4, ContactId = 3, Street = "77 Maple Dr", City = "Springfield", State = "IL", PostalCode = "62704" },
                new Address { Id = 5, ContactId = 4, Street = "5 River Way", City = "Riverside", State = "CA", PostalCode = "92501" },
                new Address { Id = 6, ContactId = 4, Street = "88 Cedar Ln", City = "Greenville", State = "SC", PostalCode = "29601" });
        }
    }
}
