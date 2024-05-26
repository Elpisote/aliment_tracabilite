import { Route } from "@angular/router";

export default [
  {
    path: '', loadComponent: () => import('./i-list/i-list.component')
      .then(mod => mod.IListComponent),
  },
  {
    path: 'update/:id', loadComponent: () => import('./i-update/i-update.component')
      .then(mod => mod.IUpdateComponent),
  }
] as Route[];
