using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aliment_backend.Entities
{
    /// <summary>
    /// Représente un enregistrement d'historique pour une entité Stock.
    /// </summary>
    public class Historical
    {
        /// <summary>
        /// Clé primaire de l'enregistrement historique.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Date du contrôle historique.
        /// </summary>
        public DateTime ControleDate { get; set; }

        /// <summary>
        /// Action effectuée lors du contrôle historique (optionnel).
        /// </summary>
        public string? Action { get; set; }

        /// <summary>
        /// Clé étrangère vers l'entité Stock associée à cet historique.
        /// </summary>
        [ForeignKey("Stock")]
        public int StockId { get; set; }

        /// <summary>
        /// Référence à l'entité Stock associée à cet historique.
        /// </summary>
        public Stock? Stock { get; set; }

        /// <summary>
        /// Constructeur de la classe Historical.
        /// </summary>
        /// <param name="controleDate">Date du contrôle historique.</param>
        /// <param name="action">Action effectuée lors du contrôle historique (optionnel).</param>
        /// <param name="stockId">Identifiant de l'entité Stock associée à cet historique.</param>
        public Historical(DateTime controleDate, string? action, int stockId)
        {
            ControleDate = controleDate;
            Action = action;
            StockId = stockId;
        }
    }
}
