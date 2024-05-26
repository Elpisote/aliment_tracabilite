import { Validators } from "@angular/forms";

/**
 * Interface définissant la configuration d'un contrôle de formulaire.
 */
export interface FormControlConfig {
  /**
   * Libellé associé au contrôle de formulaire.
   */
  label: string;

  /**
   * Nom du contrôle dans le formulaire.
   */
  controlName: string;

  /**
   * Type du contrôle de formulaire. Peut être 'text', 'number', 'select', 'group', 'email', etc.
   * Ajoutez plus de types au besoin.
   */
  type: 'text' | 'number' | 'select' | 'group' | 'email';

  /**
   * Tableau de validateurs à appliquer au contrôle de formulaire. Peut être nul.
   */
  validators?: Validators[] | null;

  /**
   * Options disponibles pour les contrôles de type 'select'.
   * Chaque option doit avoir un identifiant (id) et un libellé (label).
   */
  options?: { id: any; label: string }[];
}

