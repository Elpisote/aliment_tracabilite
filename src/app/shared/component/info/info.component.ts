import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ErrorService } from '../../_service/errorForms.service';


@Component({
  selector: 'app-info',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule
  ],
  providers: [ErrorService],
  templateUrl: './info.component.html',
  styleUrl: './info.component.css'
})

/**
  * La classe `InfoComponent` représente les informations de l'utilisateur.
  * Elle gère l'affichage et la saisie des informations personnelles telles que
  * le nom, le prénom, le nom d'utilisateur et l'adresse e-mail.
  */
export class InfoComponent {
  /** Le groupe de contrôles du formulaire d'informations personnelles. */
  formInfo!: FormGroup;

  /** Le formulaire parent auquel le composant est attaché. */
  @Input() formData!: FormGroup;

  /** Événement émis lorsqu'il y a des modifications dans le formulaire d'informations personnelles. */
  @Output() onFormGroupChange: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();

  //injection des dépendances
  fb = inject(FormBuilder)
  errorService = inject(ErrorService)

  /** 
   * Initialise le formulaire d'informations personnelles et l'ajoute au formulaire parent.
   */
  ngOnInit(): void {
    // Initialisation du formulaire d'informations personnelles avec des contrôles de validation
    this.formInfo = new FormGroup({
      lastname: this.fb.control(null, Validators.compose([
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(30)])),
      firstname: this.fb.control(null, Validators.compose([
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(30)])),
      username: this.fb.control(null, Validators.compose([
        Validators.required,
        Validators.maxLength(30)])),
      email: this.fb.control(null, Validators.compose([
        Validators.required,
        Validators.email]))
    });
    this.addGroupToParent();
  }

  /**
   * Ajoute le groupe de contrôles du formulaire d'informations personnelles au formulaire parent
   * et émet un événement pour informer le parent des changements.
   */
  private addGroupToParent(): void {
    this.formData.addControl('info', this.formInfo);

    // Émission de l'événement pour informer le formulaire parent des changements
    this.onFormGroupChange.emit(this.formData);
  }
}
