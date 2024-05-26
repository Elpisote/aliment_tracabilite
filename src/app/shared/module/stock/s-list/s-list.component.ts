import { CommonModule, DatePipe } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { Validators } from '@angular/forms';
import { FormControlConfig } from '../../../../core/_utils/FormControlConfig';
import { ListComponent } from '../../../component/list/list.component';
import { entitiesType } from '../../../enumeration/entities';
import { namePageType } from '../../../enumeration/namePage';
import { statutsType } from '../../../enumeration/statuts';
import { Stock } from '../_model/stock';

interface StatutOption {
  id: number;
  label: string;
}

@Component({
  selector: 'app-s-list',
  standalone: true,
  imports: [
    CommonModule,
    ListComponent
  ],
  providers: [DatePipe],
  templateUrl: './s-list.component.html',
  styleUrl: './s-list.component.css'
})

/*
Composant Angular utilisé pour gérer la liste des stocks.
*/
export class SListComponent {
  // Référence au composant enfant ListComponent
  @ViewChild('list') list!: ListComponent;

  // Nom de la page (utilisé pour l'affichage)
  nameType = namePageType.Stock;

  // Nom de l'entité associée à cette liste
  entityName = entitiesType.Stock;

  // Les différents types de statuts disponibles pour le stock
  types = statutsType;

  // L'entité stock à modifier
  entity: Stock = new Stock();

  // Configuration des contrôles de formulaire pour l'édition du stock
  config: FormControlConfig[] = [
    {
      label: 'Produit',                     // Libellé du contrôle
      controlName: 'product_name',          // Nom de la propriété liée au contrôle
      type: 'group',                        // Type de contrôle (groupe dans ce cas)
      validators: null,                     // Validateurs du contrôle
    },
    { label: 'Date ouverture', controlName: 'openingDate', type: 'text', validators: null }, // Contrôle pour la date d'ouverture
    {
      label: 'Statut',                       // Libellé du contrôle
      controlName: 'statuts',                // Nom de la propriété liée au contrôle
      type: 'select',                        // Type de contrôle (liste déroulante dans ce cas)
      validators: [Validators.required],     // Validateurs du contrôle
      options: []                            // Options de la liste déroulante (à remplir dynamiquement)
    }
  ];

  ngOnInit() {
    this.onSelectStatut();  // Appel de la méthode pour sélectionner le statut
  }

  /**
 * Méthode pour sélectionner le statut du stock.
 * Remplit les options de la liste déroulante avec les différents statuts disponibles.
 */
  onSelectStatut() {
    // Récupérer tous les valeurs d'énumération en tant que tableau
    const statutsValues = Object.keys(statutsType).filter((v) => isNaN(Number(v)));

    // Trouver le contrôle de statut dans le tableau de configuration
    const statutControl = this.config.find(s => s.controlName === 'statuts');
    if (statutControl) {
      // Mapper les valeurs de statut en options de liste déroulante
      statutControl.options = statutsValues.map((statutValue, index) => ({
        id: index,
        label: statutValue,
      })) as StatutOption[];
    }
  }

  /*
    Méthode pour obtenir la valeur du statut d'un stock.
    @param s: Stock - L'objet stock dont on souhaite obtenir le statut.
    @returns string - La valeur du statut sous forme de chaîne de caractères.
  */
  getStatutValue(s: Stock): string {
    return statutsType[s.statuts];
  }
}
