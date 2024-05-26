import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { AuthService } from '../../../features/auth/_service/auth.service';
import { logout } from '../../../features/auth/_state/auth-action';
import { username } from '../../../features/auth/_state/auth-selector';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatMenuModule,
    MatIconModule,    
  ],
  providers: [AuthService],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})

/**
  * Composant Angular gérant l'en-tête de la page.
  * Il inclut des fonctionnalités telles que la bascule de la barre de navigation latérale,
  * la déconnexion de l'utilisateur, et la redirection vers les informations personnelles et la modification du mot de passe.
  */
export class HeaderComponent implements OnInit {

  // Événement émis lors de la bascule de la barre de navigation latérale
  @Output() sideNavToggled = new EventEmitter<boolean>();

  // Statut actuel du menu (ouvert ou fermé)
  menuStatus: boolean = false;

  //injection des dépendances
  authService = inject(AuthService)
  router = inject(Router)
  store = inject(Store)

  // Informations sur l'utilisateur actuellement connecté
  username$: Observable<string> = of('invité');
  
  /**  
  * Récupère les informations sur l'utilisateur connecté à l'initialisation du composant.
  */
  ngOnInit(): void {
    this.username$ = this.store.select(username)
  }

  /**
   * Méthode pour basculer la barre de navigation latérale.
   * Émet un événement pour informer les autres composants du changement d'état.
   */
  sideNavToggle() {
    this.menuStatus = !this.menuStatus;
    this.sideNavToggled.emit(this.menuStatus);
  }

  /**
   * Méthode pour se déconnecter.
   * Efface le jeton d'authentification stocké.
   */
  logout (event: Event) {
    event.preventDefault();
    this.store.dispatch(logout());
  }

  /**
   * Méthode pour rediriger vers les informations personnelles en fonction du rôle de l'utilisateur.
   */
  myInformation() {
    this.authService.getCurrentRole() === 'Admin' ?
      this.router.navigate(['admin/information']) :
      this.router.navigate(['user/information']);
  }


  /**
   * Méthode pour rediriger vers la modification du mot de passe en fonction du rôle de l'utilisateur.
   */
  myPassword() {
    this.authService.getCurrentRole() === 'Admin' ?
      this.router.navigate(['admin/password/update/:id']) :
      this.router.navigate(['user/password/update/:id']);
  }
}



