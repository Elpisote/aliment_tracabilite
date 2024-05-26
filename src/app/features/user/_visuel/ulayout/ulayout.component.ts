import { Component } from '@angular/core';
import { LayoutComponent } from '../../../../shared/component/layout/layout.component';

@Component({
  selector: 'app-ulayout',
  standalone: true,
  imports: [LayoutComponent],
  templateUrl: './ulayout.component.html',
  styleUrl: './ulayout.component.css'
})

/**
  * Le composant `UlayoutComponent` représente la mise en page générale de l'application côté user.
  * Il gère l'état de la barre de navigation latérale.
  */
export class UlayoutComponent {
  /**
   * Variable qui détermine l'état actuel de la barre de navigation latérale.
   * true si la barre de navigation est ouverte, false sinon.
   */
  sideNavStatus: boolean = false;

  /**
  * Liste de menus pour la barre de navigation latérale.
  * Chaque élément de la liste a les propriétés nom, icone et lien.
  */
  listeMenu = [
    {
      nom: 'Board',
      icone: 'bi-house-door-fill',
      lien: 'stock'
    }
  ]
}
