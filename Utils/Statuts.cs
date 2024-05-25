using System.ComponentModel;

namespace aliment_backend.Utils
{
    /// <summary>
    /// Énumération représentant les différents statuts possibles.
    /// </summary>
    public enum Statuts
    {
        /// <summary>
        /// En cours.
        /// </summary>
        [Description("En cours")]
        Inprogess,

        /// <summary>
        /// Périmé.
        /// </summary>
        [Description("Périmé")]
        Expired,

        /// <summary>
        /// Erreur de saisie.
        /// </summary>
        [Description("Erreur de saisie")]
        Error,

        /// <summary>
        /// Consommé.
        /// </summary>
        [Description("Consommé")]
        Consumes
    }
}
