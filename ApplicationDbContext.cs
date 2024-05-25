using aliment_backend.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aliment_backend
{
    /// <summary>
    /// Classe DbContext pour la gestion des opérations de base de données dans l'application Aliment.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe ApplicationDbContext avec les options spécifiées.
        /// </summary>
        /// <param name="options">Les options de configuration du contexte.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Obtient ou définit le DbSet pour les catégories.
        /// </summary>
        public DbSet<Category>? Categories { get; set; }

        /// <summary>
        /// Obtient ou définit le DbSet pour les produits.
        /// </summary>
        public DbSet<Product>? Products { get; set; }

        /// <summary>
        /// Obtient ou définit le DbSet pour les stocks.
        /// </summary>
        public DbSet<Stock>? Stocks { get; set; }

        /// <summary>
        /// Obtient ou définit le DbSet pour les enregistrements historiques.
        /// </summary>
        public DbSet<Historical>? Historical { get; set; }

        /// <summary>
        /// Redéfinition du DbSet pour les utilisateurs avec le type User personnalisé.
        /// </summary>
        public override DbSet<User>? Users { get; set; }

        /// <summary>
        /// Configuration du modèle de données pour le contexte de base de données.
        /// </summary>
        /// <param name="modelBuilder">Constructeur de modèle utilisé pour construire le modèle de données.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des propriétés des entités User
            modelBuilder.Entity<User>(b =>
            {
                b.Property(u => u.Firstname)
                    .IsRequired()
                    .HasMaxLength(30);
                b.Property(u => u.Lastname)
                    .IsRequired()
                    .HasMaxLength(30);
                b.Property(u => u.UserName)
                    .HasMaxLength(30)
                    .IsRequired();
                b.Property(u => u.Email).IsRequired();

            });

            // Configuration des propriétés des entités Category
            modelBuilder.Entity<Category>(b =>
            {
                b.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            // Configuration des propriétés des entités Product
            modelBuilder.Entity<Product>(b =>
            {
                b.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(20);
                b.Property(p => p.DurationConservation).IsRequired();
            });

            // Configuration des propriétés des entités Stock
            modelBuilder.Entity<Stock>(b =>
            {
                b.Property(s => s.Statuts).IsRequired();
            });
        }
    }
}
