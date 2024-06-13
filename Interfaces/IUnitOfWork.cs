using aliment_backend.Entities;
using aliment_backend.Interfaces;

namespace aliment_backend.Interfaces
{
    /// <summary>
    /// Interface représentant un modèle de unit of work (unité de travail) pour la gestion des opérations de base de données.
    /// </summary>
    public interface IUnitOfWork 
    {
        /// <summary>
        /// Obtient le référentiel pour les entités de type Category.
        /// </summary>
        IBaseRepository<Category> Categories { get; }

        /// <summary>
        /// Obtient le référentiel pour les entités de type Product.
        /// </summary>
        IBaseRepository<Product> Products { get; }

        /// <summary>
        /// Obtient le référentiel pour les entités de type Stock.
        /// </summary>
        IBaseRepository<Stock> Stocks { get; }

        /// <summary>
        /// Obtient le référentiel pour les entités de type Historical.
        /// </summary>
        IBaseRepository<Historical> Historical { get; }

        /// <summary>
        /// Obtient le référentiel pour les entités de type User.
        /// </summary>
        IBaseRepository<User> Users { get; }

        /// <summary>
        /// Récupère le référentiel pour les entités du type spécifié.
        /// </summary>
        /// <typeparam name="TEntity">Le type d'entité pour lequel récupérer le référentiel.</typeparam>
        /// <returns>Le référentiel pour les entités du type spécifié.</returns>
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;             

        /// <summary>
        /// Enregistre les modifications dans la base de données de manière asynchrone et renvoie le nombre d'éléments affectés.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone, qui retourne le nombre d'éléments affectés dans la base de données.</returns>
        Task<int> CompleteAsync();
    }
}
