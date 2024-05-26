import { Injectable } from "@angular/core";
import { EntityCollectionServiceBase, EntityCollectionServiceElementsFactory } from "@ngrx/data";
import { entitiesType } from "../../enumeration/entities";
import { Stock } from "./_model/stock";
import { Observable, tap } from "rxjs";
import { URLBACKEND } from "../../../core/_utils/URL_BACKEND_constant";
import { HttpClient } from "@angular/common/http";


@Injectable({
  providedIn: 'root',
})

export class StockService extends EntityCollectionServiceBase<Stock> {
  constructor(
    private http: HttpClient,
    serviceElementsFactory: EntityCollectionServiceElementsFactory
  ) {
    super(entitiesType.Stock, serviceElementsFactory);
  }

  /**
   * Méthode pour créer un nouveau stock à partir d'une liste d'identifiants de produits.
   * 
   * @param productIds - Les identifiants des produits associés au stock.
   * @returns Une observable pour les stocks créés.
   */
  addStocks(productIds: number[]): Observable<Stock[]> {
    return this.http.post<Stock[]>(URLBACKEND.STOCK_ADDMANY, productIds)
      .pipe(
        tap((stocks: Stock[]) => {        
          this.addManyToCache(stocks);
        })
      );
  }
}
