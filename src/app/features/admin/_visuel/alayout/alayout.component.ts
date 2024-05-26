import { Component } from '@angular/core';
import { LayoutComponent } from '../../../../shared/component/layout/layout.component';


@Component({
  selector: 'app-alayout',
  standalone: true,
  imports: [ LayoutComponent ],
  templateUrl: './alayout.component.html',
  styleUrl: './alayout.component.css'
})

/**
* Le composant `AlayoutComponent` représente la mise en page générale de l'application côté admin.
* Il gère l'état de la barre de navigation latérale.
*/
export class AlayoutComponent {
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
    },
    {
      nom: 'Produit',
      icone: 'bi-cart4',
      lien: 'produit'
    },
    {
      nom: 'Categorie',
      icone: 'bi-diagram-3',
      lien: 'categorie'
    },
    {
      nom: 'Utilisateur',
      icone: 'bi-people-fill',
      lien: 'utilisateur'
    },
    {
      nom: 'Historique',
      icone: 'bi-arrow-counterclockwise',
      lien: 'historique'
    }
  ]
}
