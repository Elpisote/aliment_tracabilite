using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace aliment_backend.Entities
{
    /// <summary>
    /// Représente une catégorie de produits.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Clé primaire de la catégorie.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nom de la catégorie.
        /// </summary>
        [Required(ErrorMessage = "Le nom de la catégorie est requis.")]
        [MinLength(3, ErrorMessage = "Le nom de la catégorie doit comporter au moins 3 caractères.")]
        [MaxLength(20, ErrorMessage = "Le nom de la catégorie ne peut pas dépasser 20 caractères.")]
        public string? Name { get; set; }

        /// <summary>
        /// Description de la catégorie.
        /// </summary>
        [MaxLength(250, ErrorMessage = "La description de la catégorie ne peut pas dépasser 250 caractères.")]
        public string? Description { get; set; }

        /// <summary>
        /// Liste des produits appartenant à cette catégorie.
        /// </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Propriété calculée représentant le nombre de produits dans cette catégorie.
        /// </summary>
        [NotMapped]
        public int NbProduct => Products.Count;
    }
}
