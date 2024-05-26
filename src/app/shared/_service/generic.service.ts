import { Injectable, inject } from '@angular/core';
import { HistoricalService } from '../../features/admin/child/Historical/historical.service';
import { CategoryService } from '../../features/admin/child/category/category.service';
import { ProductService } from '../../features/admin/child/product/product.service';
import { UserService } from '../../features/admin/child/user-info/user.service';
import { entitiesType } from '../enumeration/entities';
import { StockService } from '../module/stock/stock.service';


@Injectable({
  providedIn: 'root'
})

/**
  * Service générique pour l'injection et la récupération des différents services.
  * 
  * Cette classe fournit des méthodes pour injecter et récupérer des services spécifiques tels que CategoryService,
  * ProductService, StockService, HistoricalService et UserService.
  */
export class GenericService {
  // injection des dépendances
  categoryService = inject(CategoryService)
  productService = inject(ProductService)
  stockService = inject(StockService)
  historicalService = inject(HistoricalService)
  userService = inject(UserService)

  /**
   * Méthode pour obtenir le service correspondant à un type d'entité donné.
   * 
   * @param entityName Le type d'entité pour lequel obtenir le service.
   * @returns Le service correspondant au type d'entité spécifié.
   * @throws Une erreur si aucun service n'est trouvé pour le type d'entité donné.
   */
  getService(entityName: entitiesType) {
    switch (entityName) {
      case entitiesType.Category:
        return this.categoryService;
      case entitiesType.Product:
        return this.productService;
      case entitiesType.Stock:
        return this.stockService;
      case entitiesType.Historical:
        return this.historicalService;
      case entitiesType.User:
        return this.userService;
      default:
        throw new Error(`Service not found for entity: ${entityName}`);
    }
  }
}
