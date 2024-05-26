import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { first, take, tap } from 'rxjs';
import { UserService } from '../../features/admin/child/user-info/user.service';
import { GenericService } from '../../shared/_service/generic.service';
import { entitiesType } from '../../shared/enumeration/entities';
import { StockService } from '../../shared/module/stock/stock.service';

/**
 * Resolver vérifiant si les données sont chargées dans le store.
 * Ce resolver s'assure que les données nécessaires sont chargées avant d'afficher une route spécifique.
 *
 * @param route Le snapshot de la route en cours.
 * @param state L'état du router.
 * @returns Un observable qui émet une valeur booléenne indiquant si les données sont chargées.
 */
export const genericResolver: ResolveFn<boolean> = (route, state) => {
  // injection des dépendances
  const genericService = inject(GenericService)
  const stockService = inject(StockService)

  // Récupération du nom de l'entité à partir des données de la route
  const entityName: entitiesType = route.data['entityName'];

  // Récupération du service générique correspondant à l'entité
  const service = genericService.getService(entityName);

  // Observable qui émettra true si les données sont chargées, sinon false
  return service.loaded$.pipe(
    tap(loaded => {
      // Si les données ne sont pas chargées, on les récupère
      if (!loaded) {
        if (entityName === entitiesType.Stock) {
          // Pour l'entité Stock, on récupère d'abord les données filtrées avant de charger toutes les données
          stockService.filteredEntities$.pipe(take(1)).subscribe(() => {
            stockService.getAll().subscribe((data: any) => console.log(data));
          });        
        } else {
          // Pour les autres entités, on charge directement toutes les données
          service.getAll()
        }
      } 
    }),
    first() // On s'assure de ne recevoir qu'une seule valeur du flux
  );
};

