import { Route } from "@angular/router";

/**
 * Configuration des routes de l'application.
 * Chaque objet représente une route avec son chemin et le chargement dynamique de son composant associé.
 */
export default [
  { path: '', redirectTo: 'login', pathMatch: 'full' }, 
  {
    path: 'login', loadComponent: () => import('./login/login.component')
      .then(mod => mod.LoginComponent)
  },
  {
    path: 'register', loadComponent: () => import('./register/register.component')
      .then(mod => mod.RegisterComponent)
  },
  {
    path: 'forgotPassword', loadComponent: () => import('./forgot-password/forgot-password.component')
      .then(mod => mod.ForgotPasswordComponent)
  },
  {
    path: 'resetPassword', loadComponent: () => import('./reset-password/reset-password.component')
      .then(mod => mod.ResetPasswordComponent)
  }

] as Route[];
