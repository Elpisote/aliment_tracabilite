import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';
import { setLoadingSpinner } from '../../../core/_global-state/action/global.action';
import { ErrorService } from '../../../shared/_service/errorForms.service';
import { AuthCardComponent } from '../../../shared/component/auth-card/auth-card.component';
import { SendMailAction } from '../_state/auth-action';
import { isSendMail } from '../_state/auth-selector';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    CommonModule,
    AuthCardComponent,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule
  ],
  providers: [ErrorService],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})

/**
  * Composant Angular gérant la fonctionnalité du mot de passe oublié.
  * Utilise les formulaires réactifs et des services personnalisés pour l'authentification,
  * la gestion des erreurs et les notifications.
  */
export class ForgotPasswordComponent implements OnInit {
  /**
  * Titre affiché dans le formulaire de récupération de mot de passe.
  */
  title = "Mot de passe oublié";

  /**
   * Libellé du bouton de soumission du formulaire.
   */
  buttonName = "Envoyer";

  /**
   * Formulaire réactif encapsulant le champ d'email et ses validations.
   */
  forgotFormGroup!: FormGroup;

  /**
   * Message de succès affiché en cas de réussite de la récupération de mot de passe.
   */
  successMessage!: string;

  /**
   * Indicateur booléen pour contrôler l'affichage conditionnel du message de succès.
   */
  sendMail$!: Observable<boolean>;
  
  //injection des dépendances
  fb = inject(FormBuilder)
  errorService = inject(ErrorService)
  store = inject(Store)
  
  /**
   * Méthode du cycle de vie Angular appelée lors de l'initialisation du composant.
   * Initialise le formulaire réactif avec le champ d'email et les validations nécessaires.
   */
  ngOnInit(): void {
    this.forgotFormGroup = this.fb.group({
      email: this.fb.control(null, Validators.compose([
        Validators.email,
        Validators.required]))
    });
    this.successMessage = 'Le lien a été envoyé, veuillez vérifier vos mails pour réinitialiser votre mot de passe.'
  }

  /**
   * Méthode appelée lors de la soumission du formulaire.
   * Récupère les données du formulaire, appelle le service d'authentification pour envoyer l'email de récupération,
   * gère les réponses avec succès ou erreur, et met à jour les propriétés du composant en conséquence.
   */
  onSubmit() {
    let forgotPassword = this.forgotFormGroup.value;
    this.store.dispatch(setLoadingSpinner({ status: true }));
    this.store.dispatch(SendMailAction.send_mail_start({ forgotPassword }));
    this.sendMail$ = this.store.pipe(select(isSendMail));
    this.successMessage = 'Le lien a été envoyé, veuillez vérifier vos mails pour réinitialiser votre mot de passe.'
  }
}
