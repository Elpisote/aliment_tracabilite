import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Store } from '@ngrx/store';
import { AuthService } from '../../../../features/auth/_service/auth.service';
import { username } from '../../../../features/auth/_state/auth-selector';
import { NotificationService } from '../../../_service/notification.service';
import { RouteService } from '../../../_service/route.service';
import { PasswordComponent } from '../../../component/password/password.component';

@Component({
  selector: 'app-pass-update',
  standalone: true,
  imports: [
    CommonModule,
    PasswordComponent,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule
  ],
  providers: [
    AuthService,
    RouteService,
    NotificationService
  ],
  templateUrl: './pass-update.component.html',
  styleUrl: './pass-update.component.css'
})

/**
  * La classe `PassUpdateComponent` est un composant Angular responsable de la gestion
  * de la mise à jour du mot de passe utilisateur. Il offre une interface pour saisir
  * le nouveau mot de passe et effectue la mise à jour en appelant le service d'authentification.
  */
export class PassUpdateComponent {
  // Formulaire pour la mise à jour du mot de passe
  passwordFormGroup!: FormGroup;

  //Contient l'identifiant de l'utilisateur actuel.
  currentUser!: string

  // Variables pour la gestion de l'affichage des caractères du mot de passe
  hide = true;
  hide2 = true;

  // injection des dépendances
  fb = inject(FormBuilder)
  store = inject(Store)
  authService = inject(AuthService)
  routeService = inject(RouteService)
  notificationService = inject(NotificationService)
    
  // Méthode appelée lors de l'initialisation du composant
  ngOnInit(): void {
    this.store.select(username).subscribe((value: string) => {
      this.currentUser = value;
    });
    this.passwordFormGroup = this.fb.group({
      username: this.currentUser
    })
  }

  // Méthode pour soumettre le formulaire (changement de mot de passe)
  onSubmit() {    
    // Récupération des données du formulaire
    let password = this.passwordFormGroup.value;

    this.authService.changePassword(password).subscribe(() => {
      // Affichage d'une notification de succès
      this.notificationService.success('Changement de mot de passe réussi');

      // Fermeture du composant et redirection vers la page d'accueil
      this.onClose();
    });    
  }

  // Méthode pour fermer le composant et rediriger vers la page d'accueil
  onClose() {
    this.routeService.goToHome();
  }
}
