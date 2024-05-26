import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MenuItem } from '../../interface/IMenuItem';
import { HeaderComponent } from '../header/header.component';
import { SideNavComponent } from '../side-nav/side-nav.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    HeaderComponent,
    SideNavComponent,    
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})

/**
* Le composant `LayoutComponent` représente la mise en page générale de l'application.
* Il gère l'état de la barre de navigation latérale.
*/
export class LayoutComponent {
  /**
    * Variable qui détermine l'état actuel de la barre de navigation latérale.
    * true si la barre de navigation est ouverte, false sinon.
    */
  sideNavStatus: boolean = false;

  /**
  * Liste dynamique des éléments de menu pour la barre de navigation latérale.
  * Chaque élément est représenté par un objet de type MenuItem.
  */
  @Input({ required: true }) listeMenu!: MenuItem[];
}
