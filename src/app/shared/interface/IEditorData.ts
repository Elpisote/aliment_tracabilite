import { FormControlConfig } from "../../core/_utils/FormControlConfig";
import { entitiesType } from "../enumeration/entities";

// Interface décrivant les données nécessaires pour ouvrir un éditeur
export interface EditorData {
  formControlConfigs: FormControlConfig[]; // Une liste de configurations de contrôle de formulaire pour le formulaire dans l'éditeur
  entityName: entitiesType; // Le type d'entité avec laquelle l'éditeur travaille, provenant de l'énumération entitiesType
  selectNameDisable?: string; // Optionnel : le nom de l'élément à désactiver dans le sélecteur
  entity: any; // L'entité à éditer, cela peut être n'importe quel type d'objet
  itemId?: any; // Optionnel : l'identifiant de l'entité à éditer, si fourni, l'éditeur est ouvert en mode de mise à jour
  isAdd: boolean; // Un indicateur booléen indiquant si l'éditeur est ouvert pour ajouter une nouvelle entité (true) ou mettre à jour une entité existante (false)
  userInfo: boolean; // Un indicateur booléen indiquant si l'éditeur est utilisé ou pas dans userInfo
}
