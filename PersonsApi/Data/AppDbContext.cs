using Microsoft.EntityFrameworkCore;
using PersonsApi.Models;

namespace PersonsApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<RelatedIndividual> RelatedIndividuals { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasOne(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasMany(p => p.PhoneNumbers)
                .WithOne(pn => pn.Person)
                .HasForeignKey(pn => pn.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Person>()
                .HasMany(p => p.RelatedIndividuals)
                .WithOne(ri => ri.Person)
                .HasForeignKey(ri => ri.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RelatedIndividual>()
                .HasOne(ri => ri.RelatedPerson)
                .WithMany()
                .HasForeignKey(ri => ri.RelatedPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Tbilisi", Country = "Georgia" },
                new City { Id = 2, Name = "Batumi", Country = "Georgia" },
                new City { Id = 3, Name = "Kutaisi", Country = "Georgia" }
            );
        }
    }
}
