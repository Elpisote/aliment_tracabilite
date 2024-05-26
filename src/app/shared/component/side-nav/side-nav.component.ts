import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MenuItem } from '../../interface/IMenuItem';

@Component({
  selector: 'app-side-nav',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './side-nav.component.html',
  styleUrl: './side-nav.component.css'
})

/**
  * Le composant `SideNavComponent` gère la barre de navigation latérale de l'application.
  * Il prend en compte le statut d'ouverture ou de fermeture de la barre de navigation (`sideNavStatus`)
  * ainsi que la liste des éléments de menu à afficher (`listeMenu`).
  */
export class SideNavComponent {
  // Indicateur du statut de la barre de navigation (ouvert ou fermé)
  @Input() sideNavStatus: boolean = false;

  // Liste des éléments de menu à afficher dans la barre latérale
  @Input({ required: true }) listeMenu!: MenuItem[];
}
