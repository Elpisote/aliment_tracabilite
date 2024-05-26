import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterModule } from '@angular/router';
import { AuthCardComponent } from '../../../shared/component/auth-card/auth-card.component';

import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { setLoadingSpinner } from '../../../core/_global-state/action/global.action';
import { LoginAction } from '../_state/auth-action';
import { ErrorService } from './../../../shared/_service/errorForms.service';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    AuthCardComponent,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule,
    RouterModule
  ],
  providers: [ErrorService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

/**
  * Composant Angular responsable de la gestion de la page de connexion.
  * Implémente OnInit pour le cycle de vie Angular.
  */
export class LoginComponent implements OnInit {

  /**
   * Titre affiché dans le formulaire de connexion.
   */
  title = "Connexion";

  /**
   * Libellé du bouton de soumission du formulaire.
   */
  buttonName = "Connexion";

  /**
   * Formulaire réactif utilisé pour la saisie des informations de connexion.
   */
  loginFormGroup!: FormGroup;

  /**
   * Booléen utilisé pour masquer ou afficher le mot de passe.
   */
  hide: boolean = true;

  //iniitalisation de l'observable
  loginFailure$!: Observable<boolean>;

  //injection des dépendances
  fb = inject(FormBuilder)
  router = inject(Router) 
  store = inject(Store)
  errorService = inject(ErrorService)
    
  /**
   * Méthode du cycle de vie Angular appelée lors de l'initialisation du composant.
   * Initialise le formulaire réactif avec les champs nécessaires et les validations.
   */
  ngOnInit(): void {
    this.loginFormGroup = this.fb.group({
      email: this.fb.control(null, Validators.compose([
        Validators.email,
        Validators.required])),
      password: this.fb.control(null, [Validators.required])
    });   
  }

  /**
   * Méthode appelée lors de la soumission du formulaire de connexion.
   * Récupère les données du formulaire, appelle le service d'authentification pour la connexion,
   * gère les réponses avec succès ou erreur, et met à jour les propriétés du composant en conséquence.
   */
  onSubmit() {
    let login = this.loginFormGroup.value;
    this.store.dispatch(setLoadingSpinner({ status: true }));
    this.store.dispatch(LoginAction.login_start({ login }));
  }

  /**
   * Méthode pour naviguer vers la page d'enregistrement.
   */
  goToRegister() {
    this.router.navigate(['/auth/register']);
  }
}
