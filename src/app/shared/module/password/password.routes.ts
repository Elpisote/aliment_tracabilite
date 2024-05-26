import { Route } from "@angular/router";

export default [ 
  {
    path: 'update/:id', loadComponent: () => import('./pass-update/pass-update.component')
      .then(mod => mod.PassUpdateComponent),
  }
] as Route[];
