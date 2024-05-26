import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { EntityCollectionServiceBase, EntityCollectionServiceElementsFactory } from "@ngrx/data";
import { Observable, catchError, throwError } from "rxjs";
import { URLBACKEND } from "../../../../core/_utils/URL_BACKEND_constant";
import { entitiesType } from "../../../../shared/enumeration/entities";
import { Product } from "../_model/product";

@Injectable({
  providedIn: 'root',
})

/**
  * Service de gestion des produits.
  * 
  * Cette classe étend EntityCollectionServiceBase<Product>, ce qui signifie qu'elle fournit des fonctionnalités pour la manipulation
  * d'une collection d'entités de type Product.
  */
export class ProductService extends EntityCollectionServiceBase<Product> {
  /**
   * Constructeur de la classe ProductService.
   * @param http Un objet de type HttpClient utilisé pour effectuer des requêtes HTTP.
   * @param serviceElementsFactory Un objet de type EntityCollectionServiceElementsFactory utilisé pour la création des services.
   */
  constructor(
    private http: HttpClient,
    serviceElementsFactory: EntityCollectionServiceElementsFactory
  ) {
    super(entitiesType.Product, serviceElementsFactory);
  }

  /**
  * Méthode pour récupérer les produits correspondant à une liste d'identifiants de catégories.
  * 
  * @param categoryIds - Liste d'identifiants de catégories.
  * @returns Une observable qui émet un tableau de produits correspondant aux catégories spécifiées.
  */
  getProductByCategories(categoryIds: number[]): Observable<Product[]> {
    // Vérifier si categoryIds est défini et non vide
    if (!categoryIds || categoryIds.length === 0) {
      // Retourner un Observable vide si categoryIds n'est pas défini ou vide
      return new Observable<Product[]>(observer => {
        observer.next([]);
        observer.complete();
      });
    }

    // Construire les paramètres de requête HTTP avec les IDs de catégorie
    let params = new HttpParams();
    for (const categoryId of categoryIds) {
      params = params.append('categoryIds', categoryId.toString());
    }

    // Utiliser HttpClient pour effectuer une requête GET avec les IDs de catégorie en tant que paramètres de requête
    return this.http.get<Product[]>(URLBACKEND.PRODUCT_GET_BY_CATEGORIES, { params: params })
      .pipe(
        catchError((error: any) => {
          console.error('Une erreur s\'est produite lors de la récupération des produits par catégories:', error);
          // Gérer l'erreur ici, par exemple retourner un Observable d'erreur ou une valeur par défaut
          return throwError('Une erreur s\'est produite lors de la récupération des produits par catégories. Veuillez réessayer plus tard.');
        })
      );
  }
}
