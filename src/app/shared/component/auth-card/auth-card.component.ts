import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';


@Component({
  selector: 'app-auth-card',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    ReactiveFormsModule,
  ],
  templateUrl: './auth-card.component.html',
  styleUrl: './auth-card.component.css'
})

/**
* Composant Angular pour une carte d'authentification.
*/
export class AuthCardComponent {

  // Titre de la carte, par défaut une chaîne vide.
  @Input({ required: true }) title: string = '';

  // Nom du bouton, par défaut une chaîne vide.
  @Input() buttonName: string = '';

  // Objet FormGroup du formulaire, doit être fourni depuis le composant parent.
  @Input({ required: true }) formGroup!: FormGroup;

  // Indicateur pour afficher ou masquer le bouton "Retour", par défaut true (afficher).
  @Input() showBackButton: boolean = true;

  // Événement émis lors de la soumission du formulaire.
  @Output() submitForm = new EventEmitter<void>();


  // injection de dépendances
  router = inject(Router)

  /**
   * Méthode appelée lors de la soumission du formulaire.
   * Émet l'événement submitForm.
   */
  onSubmit() {
    this.submitForm.emit();
    this.formGroup.reset();
    Object.keys(this.formGroup.controls).forEach(key => {
      this.formGroup.get(key)?.setErrors(null);
    });
  }

  /**
   * Méthode pour naviguer vers la page de connexion.
   */
  goToLogin() {
    this.router.navigate(['/auth/login']);
  }

  /**
   * Méthode pour naviguer vers la page d'enregistrement.
   */
  goToRegister() {
    this.router.navigate(['/auth/register']);
  }
}
