import { EntityDataModuleConfig, EntityMetadataMap } from '@ngrx/data';
import { Stock } from './shared/module/stock/_model/stock';

const entityMetadata: EntityMetadataMap = {
  Category: {}, 
  Product: {}, 
  User: {}, 
  Stock: {   
    filterFn: (entities: Stock[]) => {
      return entities.filter(entity => entity.statuts === 0)
    },   
    sortComparer(a: Stock, b: Stock): number {
      if (a.countdown === "Expiré" && b.countdown !== "Expiré") {
        return -1; // Mettre "expiré" en haut de la liste
      } else if (b.countdown === "Expiré" && a.countdown !== "Expiré") {
        return 1; // Mettre "expiré" en haut de la liste
      } else {
        const dateA = new Date(a.openingDate);
        const dateB = new Date(b.openingDate);

        // Comparer les dates d'ouverture
        return dateA.getTime() - dateB.getTime();
      }
    }
  },
  Historical: {}, 
  Role: {}  
};

const pluralNames = {
  Category: 'Categories', 
 };

export const entityConfig: EntityDataModuleConfig = {
  entityMetadata,
  pluralNames
}; 
