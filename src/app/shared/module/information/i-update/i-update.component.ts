import { CommonModule } from '@angular/common';
import { Component, Input, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { Store } from '@ngrx/store';
import { CapitalizeFirstLetterDirective } from '../../../../core/_directive/capitalizeFirstLetter.directive';
import { UpperCaseInputDirective } from '../../../../core/_directive/upperCaseInput.directive';
import { User } from '../../../../features/admin/child/_model/user';
import { UserService } from '../../../../features/admin/child/user-info/user.service';
import { CurrentUser } from '../../../../features/auth/_models/currentUser';
import { updateCurrentUser } from '../../../../features/auth/_state/auth-action';
import { NotificationService } from '../../../_service/notification.service';
import { RouteService } from '../../../_service/route.service';

@Component({
  selector: 'app-i-update',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatGridListModule,
    MatButtonModule,
    CapitalizeFirstLetterDirective,
    UpperCaseInputDirective
  ],
  providers: [
    UserService,
    RouteService,
    NotificationService
  ],
  templateUrl: './i-update.component.html',
  styleUrl: './i-update.component.css'
})

/**
  * La classe `IUpdateComponent` est un composant Angular utilisé
  * pour afficher un formulaire de mise à jour des informations utilisateur.
  */
export class IUpdateComponent {
  /** Récupération du username dans url*/
  @Input() id = ''
  
  /** Formulaire réactif pour la mise à jour des informations utilisateur */
  editFormGroup = new FormGroup({
    email: new FormControl(''),
    lastname: new FormControl(''),
    firstname: new FormControl(''),
    userName: new FormControl(''),
    role: new FormControl(),
  })

  // injection des dépendances
  userService = inject(UserService)
  store = inject(Store)
  notificationService = inject(NotificationService)
  routeService = inject(RouteService)
  
  /**
    * Méthode appelée lors de l'initialisation du composant.
    * Récupère le nom d'utilisateur à partir des paramètres de l'URL, puis utilise le service
    * utilisateur pour obtenir les informations de l'utilisateur correspondant. Les informations
    * récupérées sont utilisées pour initialiser le formulaire réactif avec les valeurs actuelles
    * de l'utilisateur.
    */
  ngOnInit(): void {  
    // Récupération des informations de l'utilisateur à partir du service utilisateur
    this.userService.getByKey(this.id).subscribe((user) => {
      if (user) { 
        // Initialisation du formulaire réactif avec les valeurs actuelles de l'utilisateur
        this.editFormGroup.patchValue({
          lastname: user.lastname,
          firstname: user.firstname,
          userName: user.userName,
          email: user.email,
          role: user.role,
        });
      }
    });
  }

  /**
   * Méthode pour mettre à jour les informations de l'utilisateur.
   * Elle met à jour les informations de l'utilisateur via le service userService,
   * met à jour le local storage avec les nouvelles informations de l'utilisateur,
   * met à jour le currentUser dans le store NgRx avec les nouvelles informations de l'utilisateur,
   * affiche une notification de succès et redirige vers la page d'informations.
   */
  onUpdateUser() {
    // Récupération des nouvelles informations de l'utilisateur depuis le formulaire
    const userData = {
      ...this.editFormGroup.value as Partial<User>,
      id: this.id,
    };

    // Appel du service userService pour mettre à jour les informations de l'utilisateur sur le serveur
    this.userService.update(userData).subscribe(() => {
      // mise à jour du localstorage
      localStorage.setItem('email', userData.email as string);
      localStorage.setItem('username', userData.userName as string)

      const newUser = new CurrentUser(
        userData.userName as string,
        userData.email as string,
        localStorage.getItem('role') as string,
        localStorage.getItem('token') as string,
        localStorage.getItem('refreshToken') as string,
      )
      // mise à jour du currentUser
      this.store.dispatch(updateCurrentUser({ currentUser: newUser }));

      // Affichage d'une notification de succès
      this.notificationService.success('Utilisateur mis à jour avec succès')

      // Redirection vers la page d'informations
      this.routeService.goToInformation();
    });   
  }

  //Redirige l'utilisateur vers la page d'accueil appropriée à la fermeture du composant.
  onClose() {
    this.routeService.goToHome();
  }
}
