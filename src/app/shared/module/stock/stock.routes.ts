import { Route } from "@angular/router";

export default [
  {
    path: '', loadComponent: () => import('./s-list/s-list.component')
      .then(mod => mod.SListComponent), 
  },
  {
    path: 'add', loadComponent: () => import('./s-add/s-add.component')
      .then(mod => mod.SAddComponent),
  }
] as Route[];
