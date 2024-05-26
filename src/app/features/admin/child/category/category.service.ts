import { Injectable } from "@angular/core";
import { EntityCollectionServiceBase, EntityCollectionServiceElementsFactory } from "@ngrx/data";
import { entitiesType } from "../../../../shared/enumeration/entities";
import { Category } from "../_model/category";

@Injectable({
  providedIn: 'root',
})

/**
 * Service de gestion des catégories.
 * 
 * Cette classe étend EntityCollectionServiceBase<Category>, ce qui signifie qu'elle fournit des fonctionnalités pour la manipulation
 * d'une collection d'entités de type Category.
 */
export class CategoryService extends EntityCollectionServiceBase<Category> {
  /**
  * Constructeur de la classe CategoryService.
  * 
  * @param serviceElementsFactory Un objet de type EntityCollectionServiceElementsFactory utilisé pour la création des services.
  */
  constructor(serviceElementsFactory: EntityCollectionServiceElementsFactory) {
    super(entitiesType.Category, serviceElementsFactory);
  }
}
