import { Route } from '@angular/router';

export default [
  {
    path: '', loadComponent: () => import('./erreur-globale/erreur-globale.component')
      .then(mod => mod.ErreurGlobaleComponent),
  },
  {
    path: 'error', loadComponent: () => import('./erreur-globale/erreur-globale.component')
      .then(mod => mod.ErreurGlobaleComponent),
  },
  {
    path: 'erreur403', loadComponent: () => import('./erreur403/erreur403.component')
      .then(mod => mod.Erreur403Component),
  },
  {
    path: 'erreur404', loadComponent: () => import('./erreur404/erreur404.component')
      .then(mod => mod.Erreur404Component),
  },
  {
    path: 'erreur500', loadComponent: () => import('./erreur500/erreur500.component')
      .then(mod => mod.Erreur500Component),
  }
] as Route[];
