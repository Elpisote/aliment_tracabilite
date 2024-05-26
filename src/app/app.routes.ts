import { Routes } from '@angular/router';
import { AdminGuard } from './core/_guard/admin.guard';
import { UserGuard } from './core/_guard/user.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'auth', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes') 
  }, 
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.routes'),
    canActivate: [AdminGuard]
  },
  {
    path: 'user',
    loadChildren: () => import('./features/user/user.routes'),
    canActivate: [UserGuard]
  },
  {
    path: 'erreur',
    loadChildren: () => import('./shared/module/erreur/erreur.routes')
  }
];
