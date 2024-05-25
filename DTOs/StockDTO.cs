using aliment_backend.Utils;
using System.ComponentModel.DataAnnotations;

namespace aliment_backend.DTOs
{
    /// <summary>
    /// Représente un objet de transfert de données (DTO) pour un stock.
    /// </summary>
    public class StockDTO
    {
        /// <summary>
        /// Statut du stock (obligatoire).
        /// </summary>
        [Required]
        public Statuts Statuts { get; set; }
        /// <summary>
        /// Obtient ou définit l'identifiant du produit auquel appartient ce stock.
        /// </summary>
        [Required]
        public int ProductId { get; set; }
    }
}
