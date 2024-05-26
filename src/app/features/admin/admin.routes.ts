import { Route } from "@angular/router";
import { genericResolver } from "../../core/_resolver/generic.resolver";
import { entitiesType } from "../../shared/enumeration/entities";

/**
 * Configuration des routes de l'application.
 * Chaque objet représente une route avec son chemin et le chargement dynamique de son composant associé.
 */

export default [
  {
    path: '', loadComponent: () => import('./_visuel/alayout/alayout.component')
      .then(mod => mod.AlayoutComponent), children: [

        { path: '', redirectTo: 'home', pathMatch: 'full' },

        {
          path: 'home', loadComponent: () => import('./ahome/ahome.component')
            .then(mod => mod.AhomeComponent)
        },
        {
          path: 'categorie', loadComponent: () => import('./child/category/c-list/c-list.component')
            .then(mod => mod.CListComponent),
          resolve: { resolver: genericResolver },
          data: { entityName: entitiesType.Category }
        },
        {
          path: 'produit', loadComponent: () => import('./child/product/p-list/p-list.component')
            .then(mod => mod.PListComponent),
          resolve: { resolver: genericResolver },
          data: { entityName: entitiesType.Product }
        },
        {
          path: 'stock', loadChildren: () => import('./../../shared/module/stock/stock.routes'),
          resolve: { resolver: genericResolver },
          data: { entityName: entitiesType.Stock }
        },
        {
          path: 'historique', loadComponent: () => import('./child/Historical/h-list/h-list.component')
            .then(mod => mod.HListComponent),
          resolve: { resolver: genericResolver },
          data: { entityName: entitiesType.Historical }
        },
        {
          path: 'utilisateur', loadComponent: () => import('./child/user-info/u-list/u-list.component')
            .then(mod => mod.UListComponent),
          resolve: { resolver: genericResolver },
          data: { entityName: entitiesType.User }
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
