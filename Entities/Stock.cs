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

        /// <summary>
        /// Temps restant avant l'expiration du stock (non mappé).
        /// </summary>
        [NotMapped]
        private TimeSpan? _remainingTime;

        /// <summary>
        /// Propriété représentant le temps restant sous forme de TimeSpan (non mappée).
        /// </summary>
        [NotMapped]
        public TimeSpan? RemainingTime
        {
            get => ExpirationDate.Subtract(DateTime.Now);
            set => _remainingTime = value;
        }

        /// <summary>
        /// Chaîne représentant le temps restant sous forme de texte (non mappée).
        /// </summary>
        [NotMapped]
        private string? _countdown;

        /// <summary>
        /// Propriété représentant le temps restant sous forme de texte (non mappée).
        /// </summary>
        [NotMapped]
        public string? Countdown
        {
            get
            {
                if (RemainingTime == null)
                    return "N/A"; // Valeur par défaut si RemainingTime est null

                int days = RemainingTime.Value.Days;
                int hours = RemainingTime.Value.Hours;
                int minutes = RemainingTime.Value.Minutes;

                if (days <= 0 && hours <= 0 && minutes <= 0)
                    _countdown = "Expiré";
                else
                    _countdown = days + " j " + hours + " h";
                return _countdown;
            }
            set => _countdown = value;
        }
    }
}
