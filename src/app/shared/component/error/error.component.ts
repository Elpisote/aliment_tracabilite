import { Component, Input, inject } from '@angular/core';
import { RouteService } from '../../_service/route.service';

@Component({
  selector: 'app-error',
  standalone: true,
  imports: [],
  templateUrl: './error.component.html',
  styleUrl: './error.component.css'
})
/**
  * Le composant `ErrorComponent` représente une interface pour afficher des messages d'erreur avec une image.
  * Il permet de personnaliser le titre, le message et l'image à afficher.
  */
export class ErrorComponent {
  // Chemin de l'image à afficher en cas d'erreur
  @Input() imagePath!: string;

  // Titre de l'erreur à afficher
  @Input() title!: string;

  // Message d'erreur détaillé à afficher
  @Input() message!: string;

  // injection des dépendances
  routeService = inject(RouteService)
 
  /**
   * Méthode appelée lorsque le bouton de fermeture est cliqué.
   * Redirige l'utilisateur vers la page d'accueil appropriée en fonction de son rôle.
   */
  closeClicked(): void {
    this.routeService.goToHome();
  }
}
