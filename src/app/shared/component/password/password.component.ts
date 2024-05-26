import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { PasswordValidators } from '../../../core/_utils/password-validators';
import { ThemeService } from '../../_service/theme.service';

/**
   * Le composant `PasswordComponent` gère la création et la validation des mots de passe
   * au sein de l'application. Il encapsule un formulaire Angular réactif pour la gestion
   * des champs de mot de passe, fournissant des fonctionnalités de validation avancées.
   * Il permet également d'intégrer ce formulaire à un formulaire parent pour une utilisation
   * modulaire et extensible.
   */
@Component({
  selector: 'app-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule
  ],
  templateUrl: './password.component.html',
  styleUrl: './password.component.css'
})
export class PasswordComponent {
  // Formulaire de gestion du mot de passe
  formPassword!: FormGroup;

  // Formulaire parent qui intègrera le formulaire de mot de passe
  @Input() formData!: FormGroup;

  // Événement émis lorsqu'il y a un changement dans le formulaire de mot de passe
  @Output() onFormGroupChange: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();

  // Indicateurs pour masquer/afficher les mots de passe
  hide: boolean = true;
  hide2: boolean = true;

  //injection des dépendances
  fb = inject(FormBuilder)
  themeService = inject(ThemeService);
 
  /**
   * Méthode appelée lors de l'initialisation du composant.
   * Initialise le formulaire de mot de passe avec des validations spécifiques,
   * permettant la création et la gestion avancée des mots de passe.
   * Ajoute également le formulaire de mot de passe au formulaire parent le cas échéant.
   */
  ngOnInit(): void {
    // Initialisation du formulaire de mot de passe avec des validations
    this.formPassword = this.fb.group({
      password: [null, Validators.compose([
        // 1. Le champ du mot de passe est requis
        Validators.required,
        // 2. Vérifie si le mot de passe contient un chiffre
        PasswordValidators.patternValidator(/\d/, { hasNumber: true }),
        // 3. Vérifie si le mot de passe contient une lettre majuscule
        PasswordValidators.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
        // 4. Vérifie si le mot de passe contient une lettre minuscule
        PasswordValidators.patternValidator(/[a-z]/, { hasSmallCase: true }),
        // 5. Vérifie si le mot de passe contient un caractère non alphanumérique
        PasswordValidators.patternValidator(/[^\w\d\s:\p{L}]/, { hasSpecialCharacters: true }),
        // 6. A une longueur minimale de 8 caractères
        Validators.minLength(8)
      ])
      ],
      confirmPassword: [null, [Validators.required]]
    },
      {
        // Vérifie si le mot de passe et la confirmation correspondent
        validator: PasswordValidators.passwordMatchValidator
      }
    );

    // Ajoute le formulaire de mot de passe au formulaire parent
    this.addGroupToParent();
  }

  // Méthode privée pour ajouter le formulaire de mot de passe au formulaire parent
  private addGroupToParent(): void {
    this.formData.addControl('password', this.formPassword);
    // Émet un événement indiquant un changement dans le formulaire
    this.onFormGroupChange.emit(this.formData);
  }
}
