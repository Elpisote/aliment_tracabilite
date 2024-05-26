import { Injectable } from "@angular/core";
import { ValidationErrors } from "@angular/forms";

@Injectable({
  providedIn: 'root'
})

/**
 * Le service `ErrorService` fournit des méthodes pour générer des messages d'erreur
 * en fonction des erreurs de validation sur les champs de formulaire.
 */
export class ErrorService {

  /**
   * Obtient un message d'erreur basé sur le type d'erreur et le champ associé.
   * 
   * @param field - Nom du champ associé à l'erreur.
   * @param error - Objet contenant des informations sur l'erreur de validation.
   * @returns Un message d'erreur associé à l'erreur spécifiée.
   */
  getErrorMessage(field: string, error: ValidationErrors): string {
    if (error['required']) {
      return "Le champ " + field + " est requis";
    }
    else if (error['minlength']) {
      return "Le champ " + field + " devrait au moins avoir " + error['minlength']['requiredLength'] + " caractères"
    }
    else if (error['maxlength']) {
      return "Le champ " + field + " ne doit pas dépasser " + error['maxlength']['requiredLength'] + " caractères"
    }
    else if (error['min']) {
      return "Le champ " + field + " doit être supérieur à " + error['min']['min']
    }
    else if (error['max']) {
      return "Le champ " + field + " doit être inférieur à " + error['max']['max']
    }
    else if (error['email']) {
      return "email invalide !"
    }
    else if (error['pattern']) {
      return "Le champ " + field + " doit être un nombre"
    }
    else return "";
  }
}
