using PopsicleFactoryCo.Models;
using Microsoft.EntityFrameworkCore;

namespace PopsicleFactoryCo.Data
{
    
    public class PopsicleDBContext : DbContext
    {
        // Constructor that accepts DbContextOptions<PopsicleDbContext> as a parameter.
        // The options parameter contains the settings required by EF Core to configure the DbContext,
        // such as the connection string and provider.
        public PopsicleDBContext(DbContextOptions<PopsicleDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed initial data for PopsicleModel
            modelBuilder.Entity<PopsicleModel>().HasData(
                new PopsicleModel
                {
                    Id = 1,
                    Name = "Nicollete path hole",
                    Flavour = "Chocolate",
                    Price = 15,
                    Quantity = 15
                },
                new PopsicleModel
                {
                    Id = 2,
                    Name = "Durham style",
                    Flavour = "Vanilla",
                    Price = 10,
                    Quantity = 10
                },
                new PopsicleModel
                {
                    Id = 3,
                    Name = "Duval style",
                    Flavour = "Strawberry",
                    Price = 20,
                    Quantity = 20
                },
                new PopsicleModel
                {
                    Id = 4,
                    Name = "Deccan style",
                    Flavour = "Orange",
                    Price = 25,
                    Quantity = 25
                },
                new PopsicleModel
                {
                    Id = 5,
                    Name = "Boulevard style",
                    Flavour = "Blueberry",
                    Price = 20,
                    Quantity = 20
                }
            );
        }

        //DbSet properties for each of your entity classes.
        public DbSet<PopsicleModel> Popsicles { get; set; }
        
    }
}

// After defining your DbContext and entity models, you need to create a migration to set up the database schema.
// Use the Package Manager Console in Visual Studio or a terminal to run the following command:
// PM> Add-Migration CreateEFCoreDB1
//To undo this action, use Remove-Migration in the Package Manager Console.
// After creating the migration, you need to apply it to the database.
// use Update-Database to apply the migration to the database.