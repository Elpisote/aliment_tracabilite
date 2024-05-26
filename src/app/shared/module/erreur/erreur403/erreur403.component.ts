import { Component } from '@angular/core';
import { ErrorComponent } from '../../../component/error/error.component';

@Component({
  selector: 'app-erreur403',
  standalone: true,
  imports: [ErrorComponent],
  templateUrl: './erreur403.component.html',
  styleUrl: './erreur403.component.css'
})

/**
  * La classe `Erreur403Component` représente un composant utilisé pour afficher une erreur 403 (accès interdit).
  * Il contient un message par défaut indiquant à l'utilisateur qu'il n'a pas l'autorisation d'accéder à la page.
  */
export class Erreur403Component {
  // Message par défaut pour l'erreur 403
  message = "Vous n'avez l'autorisation d'accéder à cette page";
}
