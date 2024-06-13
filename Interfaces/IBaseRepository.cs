using System.Linq.Expressions;

namespace aliment_backend.Interfaces
{
    /// <summary>
    /// Interface générique pour les opérations de base de données CRUD (Create, Read, Update, Delete) sur les entités de type T.
    /// </summary>
    /// <typeparam name="T">Le type d'entité.</typeparam>
    public interface IBaseRepository<T> where T : class
    {       
        /// <summary>
        /// Récupère de manière asynchrone toutes les entités de type T.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone, qui retourne une collection de toutes les entités de type T.</returns>
        public Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Récupère de manière asynchrone toutes les entités de type T qui correspondent au prédicat spécifié.
        /// </summary>
        /// <param name="predicate">Le prédicat de filtrage des entités.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, qui retourne une collection des entités de type T qui satisfont le prédicat spécifié.</returns>
        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
        
        /// <summary>
        /// Recherche les entités de type T satisfaisant les critères spécifiés.
        /// </summary>
        /// <param name="criteria">Les critères de recherche sous forme d'expression lambda.</param>
        /// <param name="includes">Un tableau de noms de propriétés à inclure.</param>
        /// <returns>Une collection des entités de type T satisfaisant les critères spécifiés.</returns>
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria);

        /// <summary>
        /// Récupère de manière asynchrone l'entité de type T avec l'identifiant spécifié.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité à récupérer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, qui retourne l'entité de type T avec l'identifiant spécifié.</returns>
        public Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Récupère l'entité de type T avec l'identifiant spécifié.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité à récupérer.</param>
        /// <returns>L'entité de type T avec l'identifiant spécifié.</returns>
        public T? GetById(int id);

        /// <summary>
        /// Ajoute de manière asynchrone une nouvelle entité de type T.
        /// </summary>
        /// <param name="entity">La nouvelle entité de type T à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, qui retourne l'entité de type T ajoutée.</returns>
        public Task<T?> AddAsynch(T entity);

        /// <summary>
        /// Met à jour une entité de type T.
        /// </summary>
        /// <param name="entity">L'entité de type T à mettre à jour.</param>
        /// <returns>L'entité de type T mise à jour.</returns>
        public T Update(T entity);

        /// <summary>
        /// Supprime de manière asynchrone l'entité de type T avec l'identifiant spécifié.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, qui retourne true si l'entité a été supprimée avec succès, sinon false.</returns>
        public Task<bool> Delete(int id);
    }
}
