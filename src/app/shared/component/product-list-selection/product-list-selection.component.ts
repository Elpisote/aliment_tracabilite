import { SelectionModel } from '@angular/cdk/collections';
import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-product-list-selection',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatIconModule,
    MatCheckboxModule
  ],
  templateUrl: './product-list-selection.component.html',
  styleUrl: './product-list-selection.component.css'
})

/**
* Composant Angular pour afficher une liste de stocks dans un tableau.
* Permet la sélection d'éléments et la personnalisation de l'affichage des colonnes.
*/
export class ProductListSelectionComponent {
  /** Source de données du tableau */
  @Input() dataSource!: MatTableDataSource<any>;
  /** Libellés des colonnes */
  @Input() columnHeaders: string[] = [];
  /** Colonnes à afficher */
  @Input() columnsToDisplay!: string[];
  /** Indique si la colonne de sélection doit être affichée */
  @Input() showSelect: boolean = true;

  /** Modèle de sélection pour la sélection des éléments */
  selection = new SelectionModel<any>(true, []);

  /** Toutes les colonnes à afficher, y compris la colonne de sélection si activée */
  allColumns: string[] = [];

  /*Couleur pour d'arrière plan pour l'entête du tableau*/
  color = 'rgba(103,58,183,0.65)';


  /** Initialisation du composant */
  ngOnInit() {
    // Détermine les colonnes à afficher en fonction de la configuration showSelect
    if (this.showSelect) {
      this.allColumns = ['select', ...this.columnsToDisplay];
    } else {
      this.allColumns = this.columnsToDisplay;
    }
  }

  //verifie si la valeur est un string
  isString(value: any): value is string {
    return typeof value === 'string';
  }

  /**
   * Obtient la valeur d'une propriété d'un objet en suivant le chemin de propriété spécifié.
   * @param item L'objet source
   * @param propertyPath Le chemin de la propriété sous forme de chaîne, avec des sous-propriétés séparées par des points
   * @returns La valeur de la propriété ou undefined si la propriété n'est pas trouvée
   */
  getItemProperty(item: any, propertyPath: string): any {
    // Divisez la chaîne des propriétés en tableau pour accéder aux sous-propriétés
    const properties = propertyPath.split('.');
    // Accédez aux sous-propriétés de manière itérative
    let value = item;
    for (const prop of properties) {
      value = value[prop];
      if (value === undefined || value === null) {
        break;
      }
    }
    return value;
  }

  /**
   * Formate la valeur d'une colonne pour affichage.
   * @param item L'élément de données à partir duquel extraire la valeur de la colonne
   * @param column La colonne à formater, pouvant être une propriété, une fonction de formatage ou le nom d'une propriété
   * @returns La valeur de la colonne formatée
   */
  formatColumnValue(item: any, column: any | Function): any {
    if (this.isString(column)) {
      // Utiliser getItemProperty pour les autres colonnes
      return this.getItemProperty(item, column);
    } else if (typeof column === 'function') {
      // Si la colonne est une fonction, l'appeler avec l'élément actuel
      return column(item);
    } else {
      // Retourner la valeur telle quelle, sans appliquer de format
      return item[column];
    }
  }

  /** Détermine si tous les éléments sont sélectionnés */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /**
   * Obtient la largeur de colonne pour une colonne spécifiée.
   * @param column Le nom de la colonne
   * @returns La largeur de la colonne
   */
  getColumnWidth(column: string): string {
    switch (column) {
      case 'id':
        return '40px'; // Largeur de la colonne 'id' en pixels
      case 'durationConservation':
        return '50px'; // Largeur de la colonne 'name' en pixels
      // Ajoutez d'autres cas pour d'autres colonnes si nécessaire
      default:
        return 'auto'; // Largeur par défaut pour les autres colonnes en pixels
    }
  }

  /** Obtient le nombre d'éléments sélectionnés */
  getSelectedItemCount(): number {
    return this.selection.selected.length;
  }
}
