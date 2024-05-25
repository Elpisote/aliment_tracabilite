using System.ComponentModel.DataAnnotations;

namespace aliment_backend.DTOs
{
    /// <summary>
    /// Représente un objet de transfert de données (DTO) pour une catégorie.
    /// </summary>
    public class CategoryDTO
    {
        /// <summary>
        /// Obtient ou définit le nom de la catégorie.
        /// </summary>        
        [Required(ErrorMessage = "Le nom de la catégorie est requis.")]
        [MinLength(3, ErrorMessage = "Le nom de la catégorie doit comporter au moins 3 caractères.")]
        [MaxLength(20, ErrorMessage = "Le nom de la catégorie ne peut pas dépasser 20 caractères.")]
        public string? Name { get; set; }

        /// <summary>
        /// Obtient ou définit la description de la catégorie.
        /// </summary>
        [MaxLength(250, ErrorMessage = "La description de la catégorie ne peut pas dépasser 250 caractères.")]
        public string? Description { get; set; }
    }
}
