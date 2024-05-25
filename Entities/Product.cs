using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aliment_backend.Entities
{
    /// <summary>
    /// Représente un produit.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Identifiant unique du produit.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nom du produit.
        /// </summary>
        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(30, ErrorMessage = "Le nom doit comporter entre {2} et {1} caractères.", MinimumLength = 3)]
        public string? Name { get; set; }

        /// <summary>
        /// Description du produit.
        /// </summary>
        [MaxLength(250)]
        public string? Description { get; set; }

        /// <summary>
        /// Durée de conservation du produit.
        /// </summary>
        [Required(ErrorMessage = "La durée de conservation est requise.")]
        [Range(1, 25, ErrorMessage = "La durée de conservation doit être comprise entre {1} et {2}.")]
        public int DurationConservation { get; set; }

        /// <summary>
        /// Clé étrangère faisant référence à la catégorie à laquelle appartient ce produit.
        /// </summary>
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Collection des stocks associés à ce produit.
        /// </summary>
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();

        /// <summary>
        /// Catégorie à laquelle appartient ce produit.
        /// </summary>
        public Category? Category { get; set; }

        /// <summary>
        /// Nombre de produits en stock pour ce produit.
        /// </summary>
        [NotMapped]
        private int _nbProductStock;

        /// <summary>
        /// Propriété calculée représentant le nombre de produits en stock pour ce produit.
        /// </summary>
        [NotMapped]
        public int NbProductStock
        {
            get => Stocks.Count;
            set => _nbProductStock = value;
        }
    }
}