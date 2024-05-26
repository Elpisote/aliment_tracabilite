import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

/**
 * Classe utilitaire contenant des validateurs personnalisés pour les champs de mot de passe.
 */
export class PasswordValidators {
  constructor() { }

  /**
   * Fonction de validateur qui utilise une expression régulière pour vérifier le motif du mot de passe.
   * @param regex Expression régulière pour la validation du motif du mot de passe.
   * @param error Objet d'erreur à retourner en cas d'échec de validation.
   * @returns Fonction de validateur conforme à l'interface ValidatorFn.
   */
  static patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (!control.value) {
        // Si la valeur du contrôle est vide, retournez null (pas d'erreur).
        return null;
      }

      // Testez la valeur du contrôle par rapport à l'expression régulière fournie.
      const valid = regex.test(control.value);

      // Si c'est vrai, retournez null (pas d'erreur), sinon retournez l'objet d'erreur passé en deuxième paramètre.
      return valid ? null : error;
    };
  }

  /**
   * Fonction de validateur qui vérifie si les champs de mot de passe correspondent.
   * @param control Contrôle de formulaire à valider.
   */
  static passwordMatchValidator(control: AbstractControl): void {
    const password: string = control?.get('password')?.value; // Obtenez le mot de passe de notre contrôle de formulaire password
    const confirmPassword: string = control?.get('confirmPassword')?.value; // Obtenez le mot de passe de notre contrôle de formulaire confirmPassword
    // Comparez si les mots de passe correspondent
    if (password !== confirmPassword) {
      // S'ils ne correspondent pas, définissez une erreur dans notre contrôle de formulaire confirmPassword
      control?.get('confirmPassword')?.setErrors({ NoPasswordMatch: true });
    }
  }
}
