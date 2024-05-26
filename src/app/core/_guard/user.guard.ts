import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../../features/auth/_service/auth.service";

// Définition d'un gardien de route pour l'administration
export const UserGuard: CanActivateFn = (state) => {
  // Injection des services nécessaires
  const authService = inject(AuthService); // Service d'authentification
  const router = inject(Router); // Service de routage

  // Vérifier si l'utilisateur est connecté
  if (authService.getIsAuthenticated()) {
    // Si l'utilisateur est connecté, vérifier s'il a le rôle d'administrateur
    if (authService.getCurrentRole() === 'User') {
      // L'utilisateur a le rôle d'administrateur, autoriser l'accès
      return true;
    } else {
      // Redirection vers une page d'erreur 403 car l'utilisateur n'est pas administrateur
      return router.createUrlTree(['/erreur/erreur403']);
    }
  } else {
    // Redirection vers la page de connexion si l'utilisateur n'est pas connecté
    return router.createUrlTree(['/Auth/login'], { queryParams: { returnUrl: state.url } });
  }
};

