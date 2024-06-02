using aliment_backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aliment_backend.Entities
{
    /// <summary>
    /// Représente une entité de stock.
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// Clé primaire de l'entité de stock.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Statut du stock (obligatoire).
        /// </summary>
        [Required]
        public Statuts Statuts { get; set; }

        /// <summary>
        /// Utilisateur ayant créé l'entité de stock.
        /// </summary>
        public string? UserCreation { get; set; }

        /// <summary>
        /// Utilisateur ayant modifié l'entité de stock.
        /// </summary>
        public string? UserModification { get; set; }

        /// <summary>
        /// Date d'ouverture du stock.
        /// </summary>
        public DateTime OpeningDate { get; set; }

        /// <summary>
        /// Clé étrangère vers l'entité Product associée à ce stock.
        /// </summary>
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        /// <summary>
        /// Liste des enregistrements historiques associés à ce stock.
        /// </summary>
        public ICollection<Historical>? Historical { get; set; }

        /// <summary>
        /// Référence à l'entité Product associée à ce stock.
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Date d'expiration du stock (non mappée).
        /// </summary>
        [NotMapped]
        public DateTime ExpirationDate { get; set; }


        [NotMapped]
        public string Countdown
        {
            get
            {
                TimeSpan remainingTime = ExpirationDate - DateTime.Now;
                if (remainingTime <= TimeSpan.Zero)
                {
                    return "Expiré";
                }
                else
                {
                    int days = remainingTime.Days;
                    int hours = remainingTime.Hours;
                    return $"{days} j {hours} h";
                }
            }
        }
    }
}
