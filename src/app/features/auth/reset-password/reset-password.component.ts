import { CommonModule } from '@angular/common';
import { Component, Input, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { setLoadingSpinner } from '../../../core/_global-state/action/global.action';
import { AuthCardComponent } from '../../../shared/component/auth-card/auth-card.component';
import { PasswordComponent } from '../../../shared/component/password/password.component';
import { ResetPasswordAction } from '../_state/auth-action';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    CommonModule,
    AuthCardComponent,
    PasswordComponent,
    ReactiveFormsModule
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})

/**
* Composant Angular responsable de la réinitialisation du mot de passe.
* Utilise le formulaire réactif pour saisir le nouveau mot de passe.
*/
export class ResetPasswordComponent {
  /**
  * Titre affiché dans le formulaire de récupération de mot de passe.
  */
  title = "Changement de mot de passe";

  /**
   * Libellé du bouton de soumission du formulaire.
   */
  buttonName = "Envoyer";
  
  /**
   * Formulaire réactif utilisé pour la saisie du nouveau mot de passe.
   */
  passwordFormGroup!: FormGroup;

  /**
   * Variable de contrôle pour masquer ou afficher le mot de passe.
   */
  hide = true;

  /**
   * Variable de contrôle pour masquer ou afficher la confirmation du mot de passe.
   */
  hide2 = true;

  //injection des dépendances
  fb = inject(FormBuilder)
  store = inject(Store)

  @Input() email: string = ''
  @Input() token: string = ''

  /**
   * Méthode du cycle de vie Angular appelée lors de l'initialisation du composant.
   * Initialise le formulaire réactif avec les paramètres de l'URL.
   */
  ngOnInit(): void {
    this.passwordFormGroup = this.fb.group({
      email: this.email,
      token: this.token
    });
  }

  /**
   * Méthode appelée lors de la soumission du formulaire de réinitialisation du mot de passe.
   * Récupère les données du formulaire, appelle le service d'authentification pour la réinitialisation,
   * gère les réponses avec succès ou erreur, et affiche des notifications en conséquence.
   */
  onSubmit() {
    let resetPassword = this.passwordFormGroup.value;
    this.store.dispatch(setLoadingSpinner({ status: true }));
    this.store.dispatch(ResetPasswordAction.reset_password_start({ resetPassword }));    
  }    
}
