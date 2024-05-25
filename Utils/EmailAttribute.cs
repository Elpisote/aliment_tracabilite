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
            // Vérifier si la valeur est null ou vide
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return false; // La valeur est nulle ou vide, donc invalide

            string? email = value.ToString();

            // Modèle d'expression régulière pour valider un email
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            if (email != null)
                // Vérification de correspondance avec le modèle d'expression régulière
                return Regex.IsMatch(email, pattern);
            return false;
        }
    }
}