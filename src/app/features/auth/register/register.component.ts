import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { setLoadingSpinner } from '../../../core/_global-state/action/global.action';
import { AuthCardComponent } from '../../../shared/component/auth-card/auth-card.component';
import { InfoComponent } from '../../../shared/component/info/info.component';
import { PasswordComponent } from '../../../shared/component/password/password.component';
import { SignupAction } from '../_state/auth-action';


@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    AuthCardComponent,
    InfoComponent,
    PasswordComponent,
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})

/**
  * Composant Angular responsable de la gestion de la page d'enregistrement.
  * Implémente OnInit pour le cycle de vie Angular.
  */
export class RegisterComponent {
  /**
   * Titre affiché dans le formulaire d'enregistrement.
   */
  title = "Enregistrement";

  /**
   * Libellé du bouton de soumission du formulaire.
   */
  buttonName = "Enregistrement";

  /**
   * Formulaire réactif utilisé pour la saisie des informations d'enregistrement.
   */
  registerFormGroup!: FormGroup;

  // injection des dépendances
  fb = inject(FormBuilder)
  store = inject(Store)

  /**
   * Méthode du cycle de vie Angular appelée lors de l'initialisation du composant.
   * Initialise le formulaire réactif avec un objet vide.
   */
  ngOnInit(): void {
    this.registerFormGroup = this.fb.group({});
  }

  /**
    * Méthode appelée lors de la soumission du formulaire d'enregistrement.
    * Récupère les données du formulaire, appelle le service d'authentification pour l'enregistrement,
    * gère les réponses avec succès ou erreur, et met à jour les propriétés du composant en conséquence.
    */
  onSubmit() {
    let register = this.registerFormGroup.value;
    this.store.dispatch(setLoadingSpinner({ status: true }));
    this.store.dispatch(SignupAction.signup_start({ register }));
  }
}
