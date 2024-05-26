import { Injectable } from "@angular/core";
import { EntityCollectionServiceBase, EntityCollectionServiceElementsFactory } from "@ngrx/data";
import { entitiesType } from "../../../../shared/enumeration/entities";
import { Historical } from "../_model/historical";

@Injectable({
  providedIn: 'root',
})

/**
  * Service de gestion des historiques.
  * 
  * Cette classe étend EntityCollectionServiceBase<Historical>, ce qui signifie qu'elle fournit des fonctionnalités pour la manipulation
  * d'une collection d'entités de type Historical.
  */
export class HistoricalService extends EntityCollectionServiceBase<Historical> {
  /**
 * Constructeur de la classe HistoricalService.
 * 
 * @param serviceElementsFactory Un objet de type EntityCollectionServiceElementsFactory utilisé pour la création des services.
 */
  constructor(serviceElementsFactory: EntityCollectionServiceElementsFactory) {
    super(entitiesType.Historical, serviceElementsFactory);
  }
}
