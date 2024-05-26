import { Injectable, inject } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { AuthService } from "../../features/auth/_service/auth.service";
import { role } from "../../features/auth/_state/auth-selector";


@Injectable({
  providedIn: 'root'
})

/**
 * Le service `RouteService` fournit des méthodes pour la navigation dans l'application
 * en fonction du rôle de l'utilisateur et du type d'entité.
 */
export class RouteService {
  // injection des dépendances
  router = inject(Router)
  authService = inject(AuthService)
  store = inject(Store)


  /**
    * Navigue vers la page d'accueil en fonction du rôle de l'utilisateur.
    * Si l'utilisateur a le rôle 'user', il est redirigé vers la page d'accueil des utilisateurs.
    * Sinon, s'il a le rôle 'admin', il est redirigé vers la page d'accueil des administrateurs.
    * Cette méthode utilise le service d'authentification pour récupérer le rôle actuel de l'utilisateur.
    */
  goToHome(): void {
    // Obtient le rôle actuel de l'utilisateur à partir du sélecteur
    this.store.select(role).subscribe(userRole => {
      const redirectRoute = (userRole === 'Admin') ? 'admin/home' : 'user/home';      
      this.router.navigate([redirectRoute]);
    });
  }

  /**
 * Navigue vers la page d'informations en fonction du rôle de l'utilisateur, avec possibilité de mise à jour.
 * Si l'utilisateur a le rôle 'user', il est redirigé vers la page d'informations pour les utilisateurs.
 * Sinon, s'il a le rôle 'admin', il est redirigé vers la page d'informations pour les administrateurs.
 * 
 * @param {string} id - L'identifiant de l'entité.
 * @param {boolean} isUpdate - Indique s'il s'agit d'une mise à jour ou simplement de consultation.
 */
  goToInformation(id?: string, isUpdate?: boolean): void {
    // Obtient le rôle actuel de l'utilisateur à partir du service d'authentification
    const userRole = this.authService.getCurrentRole();

    // Détermine la route de redirection en fonction du rôle de l'utilisateur et de la nature de l'action (mise à jour ou consultation)
    let redirectRoute: string;
    if (userRole === 'User') {
      redirectRoute = isUpdate ? `user/information/update/${id}` : 'user/information';
    } else {
      redirectRoute = isUpdate ? `admin/information/update/${id}` : 'admin/information';
    }

    // Redirige l'utilisateur vers la route déterminée
    this.router.navigate([redirectRoute]);
  }
}


