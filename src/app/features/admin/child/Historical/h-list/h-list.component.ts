import { CommonModule, DatePipe, TitleCasePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ListComponent } from '../../../../../shared/component/list/list.component';
import { entitiesType } from '../../../../../shared/enumeration/entities';
import { namePageType } from '../../../../../shared/enumeration/namePage';
import { statutsType } from '../../../../../shared/enumeration/statuts';
import { Historical } from '../../_model/historical';

@Component({
  selector: 'app-h-list',
  standalone: true,
  imports: [
    CommonModule,
    ListComponent
  ],
  providers: [
    DatePipe,
    TitleCasePipe
  ],
  templateUrl: './h-list.component.html',
  styleUrl: './h-list.component.css'
})

/**
* La classe `HListComponent` représente le composant de liste pour les historiques (Historical).
* Elle gère l'affichage d'une liste d'historiques avec des colonnes spécifiques.
*/
export class HListComponent {
  /**
   * Le type de nom utilisé, généralement utilisé comme titre de la liste (dans ce cas, "Historical").
   */
  nameType = namePageType.Historical;

  /**
   * Le nom de l'entité associée à la liste (dans ce cas, "Historical").
   */
  entityName = entitiesType.Historical;

  /**
   * Méthode pour obtenir la valeur de l'utilisateur en fonction de l'action de l'historique.
   * @param h - L'historique pour lequel obtenir la valeur de l'utilisateur.
   * @returns La valeur de l'utilisateur en fonction de l'action.
   */
  getUtilisateurValue(h: Historical): string {
    // Logique conditionnelle pour déterminer la valeur de l'utilisateur en fonction de l'action
    if (h.action === 'Création') {
      return h.stock.userCreation;
    } else if (h.action === 'Modification') {
      return h.stock.userModification;
    }
    return '';
  }

  /**
   * Méthode pour obtenir la valeur du statut en fonction de l'action de l'historique.
   * @param h - L'historique pour lequel obtenir la valeur du statut.
   * @returns La valeur du statut en fonction de l'action.
   */
  getStatutValue(h: Historical): string {
    // Logique conditionnelle pour déterminer la valeur du statut en fonction de l'action
    if (h.action === 'Création') {
      return 'En cours';
    } else if (h.action === 'Modification') {
      return statutsType[h.stock.statuts];
    }
    return '';
  }
}
