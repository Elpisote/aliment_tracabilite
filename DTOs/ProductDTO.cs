using System.ComponentModel.DataAnnotations;

namespace aliment_backend.DTOs
{
    /// <summary>
    /// Représente un objet de transfert de données (DTO) pour un produit.
    /// </summary>
    public class ProductDTO
    {
        /// <summary>
        /// Obtient ou définit le nom du produit.
        /// </summary>
        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(30, ErrorMessage = "Le nom doit comporter entre {2} et {1} caractères.", MinimumLength = 3)]
        public string? Name { get; set; }

        /// <summary>
        /// Obtient ou définit la description du produit.
        /// </summary>
        [MaxLength(250, ErrorMessage = "La description du produit ne peut pas dépasser 250 caractères.")]
        public string? Description { get; set; }

        /// <summary>
        /// Obtient ou définit la durée de conservation du produit.
        /// </summary>
        [Required(ErrorMessage = "La durée de conservation est requise.")]
        [Range(1, 25, ErrorMessage = "La durée de conservation doit être comprise entre 1 et 25.")]
        public int DurationConservation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la catégorie à laquelle appartient ce produit.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }           
    }
}
