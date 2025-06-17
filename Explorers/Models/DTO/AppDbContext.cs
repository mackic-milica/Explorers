using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SredaZadatak.Models.DTO
{
    public class AppDbContext :  IdentityDbContext<ApplicationUser>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Explorer> Explorers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Explorer>(entity =>
        {
            entity.Property(e => e.Salary)
                  .HasPrecision(18, 2); // za dodavanje preciznosti

            entity.HasOne(e => e.Project)
             .WithMany(p => p.Explorers)
             .HasForeignKey(e => e.ProjectId); // da poveze projekat i istrazivaca
        });

            modelBuilder.Entity<Project>().HasData(
                new Project() { Id = 1, Name = "Istrazivanje Trzista", StartYear = 2021, EndYear = 2023 },
                new Project() { Id = 2, Name = "Decije proteze", StartYear = 2005, EndYear = 2021 },
                new Project() { Id = 3, Name = "Posumljavanje okeana", StartYear = 2003, EndYear = 2015 }

                );

            modelBuilder.Entity<Explorer>().HasData(
                new Explorer() { Id = 1, Name = "Aleksandra", LastName = "Petrovic", BirthYear = 1994, Salary = 450000.0m, ProjectId = 1, },
                new Explorer() { Id = 2, Name = "Ljubomir", LastName = "Rakic", BirthYear = 1996, Salary = 430000.0m, ProjectId = 2 },
                new Explorer() { Id = 3, Name = "Jasmina", LastName = "Rakic", BirthYear = 1961, Salary = 340000.0m, ProjectId = 2 },
                new Explorer() { Id = 4, Name = "Dina", LastName = "Kucic", BirthYear = 2002, Salary = 120000.0m, ProjectId = 3 }
                );


            base.OnModelCreating(modelBuilder);
        }
    }
}
