import { CommonModule, DatePipe, TitleCasePipe } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { Validators } from '@angular/forms';
import { FormControlConfig } from '../../../../../core/_utils/FormControlConfig';
import { ListComponent } from '../../../../../shared/component/list/list.component';
import { entitiesType } from '../../../../../shared/enumeration/entities';
import { namePageType } from '../../../../../shared/enumeration/namePage';
import { Category } from '../../_model/category';

@Component({
  selector: 'app-c-list',
  standalone: true,
  imports: [
    CommonModule,
    ListComponent 
  ],
  providers: [
    DatePipe,
    TitleCasePipe
  ],
  templateUrl: './c-list.component.html',
  styleUrl: './c-list.component.css'
})
/**
 * La classe `CListComponent` représente le composant de liste pour les catégories (Category).
 * Elle gère l'affichage d'une liste d'entités, en utilisant le composant `ListComponent`.
 */
export class CListComponent {
  /**
   * Référence au composant `ListComponent`, utilisée pour accéder aux méthodes et propriétés de ce composant.
   * Cette référence est obtenue grâce à la directive `ViewChild`.
   */
  @ViewChild('list') list!: ListComponent;

  /**
   * Le type de nom de la page, utilisé généralement comme titre de la page.
   */
  nameType = namePageType.Category;

  /**
   * Le nom de l'entité associée à la liste (dans ce cas, une catégorie).
   */
  entityName = entitiesType.Category;

  /**
   * Entité qui sera affichée dans le formulaire. Initialisée en tant que nouvelle instance de la classe Category.
   */
  entity: Category = new Category();

  /**
   * Configuration des contrôles du formulaire.
   * Chaque objet de cette liste représente un contrôle du formulaire avec des propriétés telles que le label, le nom du contrôle, le type, et les validateurs.
   */
  config: FormControlConfig[] = [
    { label: 'Nom', controlName: 'name', type: 'text', validators: [Validators.required, Validators.minLength(3), Validators.maxLength(20)] },
    { label: 'Description', controlName: 'description', type: 'text', validators: null }];
}
