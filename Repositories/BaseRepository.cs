using aliment_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace aliment_backend.Repositories
{
    /// <summary>
    /// Implémentation de base d'un référentiel générique pour effectuer des opérations CRUD sur les entités.
    /// </summary>
    /// <typeparam name="T">Le type d'entité pour lequel ce référentiel est défini.</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Initialise une nouvelle instance de la classe BaseRepository avec le contexte de base de données spécifié.
        /// </summary>
        /// <param name="context">Le contexte de base de données.</param>
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }       

        /// <summary>
        /// Récupère de manière asynchrone toutes les entités de type T.
        /// </summary>
        /// <returns>Une tâche représentant une collection asynchrone de toutes les entités de type T.</returns>
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        /// <summary>
        /// Récupère de manière asynchrone toutes les entités de type T satisfaisant le prédicat spécifié.
        /// Si aucun prédicat n'est fourni, récupère toutes les entités de type T.
        /// </summary>
        /// <param name="predicate">Le prédicat pour filtrer les entités (facultatif).</param>
        /// <returns>Une tâche représentant une collection asynchrone de toutes les entités de type T satisfaisant le prédicat spécifié.</returns>
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
        {
            IQueryable<T> query = _dbSet;

            // Appliquer le prédicat s'il est fourni
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // Exécuter la requête et retourner les résultats
            return await query.ToListAsync();
        }

        /// <summary>
        /// Récupère de manière asynchrone une entité de type T à partir de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité à récupérer.</param>
        /// <returns>Une tâche représentant l'entité de type T trouvée, ou null si aucune entité correspondante n'est trouvée.</returns>
        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        /// <summary>
        /// Récupère de manière synchrone une entité de type T à partir de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité à récupérer.</param>
        /// <returns>L'entité de type T trouvée, ou null si aucune entité correspondante n'est trouvée.</returns>
        public T? GetById(int id) => _context.Set<T>().Find(id);

        // <summary>
        /// Récupère toutes les entités de type T satisfaisant le critère spécifié, en incluant éventuellement les entités associées spécifiées.
        /// </summary>
        /// <param name="criteria">Le critère de filtrage des entités.</param>
        /// <returns>Une collection contenant toutes les entités de type T satisfaisant le critère spécifié.</returns>
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _context.Set<T>();
            return query.Where(criteria).ToList();
        }

        /// <summary>
        /// Ajoute de manière asynchrone une nouvelle entité de type T dans le référentiel.
        /// </summary>
        /// <param name="entity">L'entité de type T à ajouter.</param>
        /// <returns>Une tâche représentant l'opération d'ajout avec l'entité ajoutée.</returns>
        public async Task<T?> AddAsynch(T entity)
        {
            await _dbSet.AddAsync(entity);          
            return entity;
        }

        /// <summary>
        /// Met à jour une entité de type T dans le référentiel.
        /// </summary>
        /// <param name="entity">L'entité de type T à mettre à jour.</param>
        /// <returns>L'entité de type T mise à jour.</returns>
        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }

        /// <summary>
        /// Supprime de manière asynchrone une entité de type T du référentiel en utilisant son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'entité à supprimer.</param>
        /// <returns>Une tâche représentant l'opération de suppression.</returns>
        public async Task<bool> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                return true;
            }
            return false;            
        }     
    }
}
