import { Injectable } from "@angular/core";
import { EntityCollectionServiceBase, EntityCollectionServiceElementsFactory } from "@ngrx/data";
import { Role } from "../_model/role";

@Injectable({
  providedIn: 'root',
})
/**
  * Service de gestion des roles.
  * 
  * Cette classe étend EntityCollectionServiceBase<Role>, ce qui signifie qu'elle fournit des fonctionnalités pour la manipulation
  * d'une collection d'entités de type Role.
  */
export class RoleService extends EntityCollectionServiceBase<Role> {
  /**
 * Constructeur de la classe RoleService.
 * 
 * @param serviceElementsFactory Un objet de type EntityCollectionServiceElementsFactory utilisé pour la création des services.
 */
  constructor(serviceElementsFactory: EntityCollectionServiceElementsFactory) {
    super('Role', serviceElementsFactory);
  }
}
