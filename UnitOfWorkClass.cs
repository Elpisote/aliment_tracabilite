using aliment_backend;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using aliment_backend.Repositories;

namespace aliment_backend
{
    /// <summary>
    /// Classe UnitOfWork responsable de la coordination des opérations de base de données pour différentes entités.
    /// </summary>
    public class UnitOfWorkClass : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Obtient ou définit le référentiel pour les catégories.
        /// </summary>
        public IBaseRepository<Category> Categories { get; private set; }

        /// <summary>
        /// Obtient ou définit le référentiel pour les produits.
        /// </summary>
        public IBaseRepository<Product> Products { get; private set; }

        /// <summary>
        /// Obtient ou définit le référentiel pour les stocks.
        /// </summary>
        public IBaseRepository<Stock> Stocks { get; private set; }

        /// <summary>
        /// Obtient ou définit le référentiel pour les enregistrements historiques.
        /// </summary>
        public IBaseRepository<Historical> Historical { get; private set; }

        /// <summary>
        /// Obtient ou définit le référentiel pour les utilisateurs.
        /// </summary>
        public IBaseRepository<User> Users { get; private set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe UnitOfWorkClass.
        /// </summary>
        /// <param name="context">Le contexte de base de données.</param>
        public UnitOfWorkClass(ApplicationDbContext context)
        {
            _context = context;
            Categories = new BaseRepository<Category>(_context);
            Products = new BaseRepository<Product>(_context);   
            Stocks = new BaseRepository<Stock>(_context);
            Historical = new BaseRepository<Historical>(_context);
            Users = new BaseRepository<User>(_context);            
        }

        /// <summary>
        /// Obtient le référentiel spécifique pour le type d'entité demandé.
        /// </summary>
        /// <typeparam name="TEntity">Le type de l'entité.</typeparam>
        /// <returns>Le référentiel pour le type d'entité spécifié.</returns>
        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            // Ici, nous décidons dynamiquement du référentiel en fonction du type TEntity demandé
             if (typeof(TEntity) == typeof(Category))
                 return (IBaseRepository<TEntity>)Categories;
            else if (typeof(TEntity) == typeof(Product))
                 return (IBaseRepository<TEntity>)Products;
            else if (typeof(TEntity) == typeof(Stock))
                 return (IBaseRepository<TEntity>)Stocks; 
            else if (typeof(TEntity) == typeof(Historical))
                 return (IBaseRepository<TEntity>)Historical; 
            else if (typeof(TEntity) == typeof(User))
                 return (IBaseRepository<TEntity>)Users;
            else
                 throw new ArgumentException($"Type d'entité non pris en charge: {typeof(TEntity).Name}");                   
        }

       
        /// <summary>
        /// Enregistre de manière asynchrone les modifications apportées dans le contexte dans la base de données.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone, qui retourne le nombre d'entités affectées.</returns>
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
