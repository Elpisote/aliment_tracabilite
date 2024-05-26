import { Route } from "@angular/router";
import { genericResolver } from "../../core/_resolver/generic.resolver";
import { entitiesType } from "../../shared/enumeration/entities";

/**
 * Configuration des routes de l'application.
 * Chaque objet représente une route avec son chemin et le chargement dynamique de son composant associé.
 */

export default [
  {
    path: '', loadComponent: () => import('./_visuel/ulayout/ulayout.component')
      .then(mod => mod.UlayoutComponent), children: [

        { path: '', redirectTo: 'home', pathMatch: 'full' },

        {
          path: 'home', loadComponent: () => import('./uhome/uhome.component')
            .then(mod => mod.UhomeComponent)          
        },        
        {
          path: 'stock', loadChildren: () => import('./../../shared/module/stock/stock.routes'),
          resolve: { resolver: genericResolver },
          data: { entityName: entitiesType.Stock }
        },        
        {
          path: 'information', loadChildren: () => import('./../../shared/module/information/information.routes')
        },
        {
          path: 'password', loadChildren: () => import('./../../shared/module/password/password.routes')
        }
      ]
  }
] as Route[];
