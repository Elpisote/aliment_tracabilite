import { Directive, ElementRef, HostListener, Optional, inject } from "@angular/core";
import { NgControl } from "@angular/forms";

@Directive({
  selector: '[upperCaseInputDirective]',
  standalone: true
})
export class UpperCaseInputDirective {
  // injection des dépendances
  element = inject(ElementRef)
  @Optional() ngControl = inject(NgControl)
  
  // Écouteur d'événement pour l'événement 'input' (lorsque l'utilisateur saisit dans l'élément)
  @HostListener('input') onInput() {
    // Récupération de la valeur saisie dans l'élément
    const value: string = this.element.nativeElement.value;

    // Vérification si la valeur est définie et non vide
    if (value) {
      // Conversion de la valeur en majuscules
      const uppercasedValue: string = value.toUpperCase();

      // Mise à jour de la valeur de l'élément avec la version en majuscules
      this.element.nativeElement.value = uppercasedValue;

      // Mettre à jour la valeur du formulaire associé (le cas échéant)
      if (this.ngControl && this.ngControl.control) {
        this.ngControl.control.setValue(uppercasedValue);
      }
    }
  }
}
