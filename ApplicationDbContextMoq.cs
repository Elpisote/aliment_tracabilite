using aliment_backend;
using aliment_backend.Entities;
using aliment_backend.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace unit_test
{
    public class ApplicationDbContextMoq
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
        public ApplicationDbContext Context { get; }

        public ApplicationDbContextMoq()
        {
            // Initialise la connexion à une base de données en mémoire (SQLite)
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // Configure les options du contexte de base de données
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseSqlite(_connection)
                                .Options;

            // Initialise le contexte de base de données avec les options configurées
            Context = new ApplicationDbContext(_contextOptions);

            // Assurez-vous que la base de données est créée
            Context.Database.EnsureCreated();

            // Ajoutez des données fictives à la base de données
            AddTestData();
        }

        private void AddTestData()
        {
            List<Category> categories = new()
            {
                new Category { Id = 1, Name = "Viande", Description = "" },
                new Category { Id = 2, Name = "Poisson", Description = "blanc" },
                new Category { Id = 3, Name = "Volaille", Description = "" },
                new Category { Id = 4, Name = "Légume", Description = "vert" },
                new Category { Id = 5, Name = "Produit laitier", Description = "" }
            };

            List<Product> products = new()
            {
                new Product { Id = 1, Name = "Boeuf", Description = "", DurationConservation = 5, CategoryId = 1 },
                new Product { Id = 2, Name = "Saumon", Description = "", DurationConservation = 3, CategoryId = 2 },
                new Product { Id = 3, Name = "Poulet", Description = "", DurationConservation = 4, CategoryId = 3 },
                new Product { Id = 4, Name = "Agneau", Description = "", DurationConservation = 3, CategoryId = 1 },
                new Product { Id = 5, Name = "Haricot vert", Description = "", DurationConservation = 5, CategoryId = 4 },
            };

            List<Stock> stocks = new()
            {
                new Stock { Id = 1, Statuts = Statuts.Inprogess, UserCreation="evetaylor", UserModification="johndoe", OpeningDate=DateTime.Now.AddDays(-1), ProductId=1 },
                new Stock { Id = 2, Statuts = Statuts.Inprogess, UserCreation="evetaylor", UserModification="johndoe", OpeningDate=DateTime.Now.AddDays(-1), ProductId=2 },
                new Stock { Id = 3, Statuts = Statuts.Error, UserCreation="evetaylor", UserModification="johndoe", OpeningDate=DateTime.Now.AddDays(-4), ProductId=2 },
                new Stock { Id = 4, Statuts = Statuts.Expired, UserCreation="evetaylor", UserModification="johndoe", OpeningDate=DateTime.Now.AddDays(-6), ProductId=5 }
            };

            List<Historical> historicals = new()
            {
                new Historical (DateTime.Now, "Création", 1),
                new Historical (DateTime.Now.AddDays(+1), "Modification", 1),
                new Historical (DateTime.Now.AddDays(-2), "Création", 2),
                new Historical (DateTime.Now, "Modification", 2)
            };

            List<IdentityRole> roles = new()
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };

            List<User> users = new()
            {
                new User { Id = "1", Firstname = "John", Lastname = "Doe", Email = "john@example.com", UserName = "johndoe" },
                new User { Id = "2",Firstname = "Jane", Lastname = "Smith", Email = "jane@example.com", UserName = "janesmith" },
                new User { Id = "3",Firstname = "Alice", Lastname = "Johnson", Email = "alice@example.com", UserName = "alicejohnson" },
                new User { Id = "4",Firstname = "Bob", Lastname = "Brown", Email = "bob@example.com", UserName = "bobbrown" },
                new User { Id = "5",Firstname = "Eve", Lastname = "Taylor", Email = "eve@example.com", UserName = "evetaylor" },
                new User { Id = "6",Firstname = "Test", Lastname = "AdminTest", Email = "admin@gmail.com", UserName = "Administrator" }
            };

            Context?.Categories?.AddRange(categories);
            Context?.Products?.AddRange(products);
            Context?.Stocks?.AddRange(stocks);
            Context?.Historical?.AddRange(historicals);
            Context?.Roles?.AddRange(roles);
            Context?.Users?.AddRange(users);
            Context?.SaveChanges();
        }      
    }
}