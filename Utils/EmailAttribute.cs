using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace aliment_backend.Utils
{
    /// <summary>
    /// Attribut de validation personnalisé pour valider une adresse e-mail.
    /// </summary>
    public class EmailAttribute : ValidationAttribute
    {
        /// <summary>
        /// Valide si la valeur spécifiée est une adresse e-mail valide.
        /// </summary>
        /// <param name="value">La valeur à valider.</param>
        /// <returns>True si la valeur est une adresse e-mail valide, sinon False.</returns>
        public override bool IsValid(object? value)
        {
            // Vérifier si la valeur est null
            if (value == null)
                return false; 

            string? email = value.ToString();

            // Vérifier si la valeur convertie en string est vide ou composée uniquement d'espaces
            if (string.IsNullOrWhiteSpace(email))
                return false; // La valeur est nulle, vide ou composée uniquement d'espaces, donc invalide

            // Modèle d'expression régulière pour valider un email
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            // Vérification de correspondance avec le modèle d'expression régulière
            return Regex.IsMatch(email, pattern);
        }
    }
}