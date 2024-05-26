import { Directive, ElementRef, HostListener, inject } from "@angular/core";

@Directive({
  selector: '[capitalizeFirstLetter]',
  standalone: true
})
export class CapitalizeFirstLetterDirective {
  // injection des dépendances
  element = inject(ElementRef)

  // Écouteur d'événement pour l'événement 'input' (lorsque l'utilisateur saisit dans l'élément)
  @HostListener('input') onInput() {
    // Récupération de la valeur saisie dans l'élément
    const value: string = this.element.nativeElement.value;

    // Vérification si la valeur est définie et non vide
    if (value) {
      // Modification de la première lettre en majuscule et concaténation avec le reste de la chaîne
      this.element.nativeElement.value = value.charAt(0).toUpperCase() + value.slice(1);
    }
  }
}
