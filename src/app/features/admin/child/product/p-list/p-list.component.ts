import { CommonModule, DatePipe, TitleCasePipe } from '@angular/common';
import { Component, ViewChild, inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { FormControlConfig } from '../../../../../core/_utils/FormControlConfig';
import { ListComponent } from '../../../../../shared/component/list/list.component';
import { entitiesType } from '../../../../../shared/enumeration/entities';
import { namePageType } from '../../../../../shared/enumeration/namePage';
import { Category } from '../../_model/category';
import { Product } from '../../_model/product';
import { CategoryService } from '../../category/category.service';

@Component({
  selector: 'app-p-list',
  standalone: true,
  imports: [
    CommonModule,
    ListComponent
  ],
  providers: [
    DatePipe,
    TitleCasePipe,
    CategoryService
  ],
  templateUrl: './p-list.component.html',
  styleUrl: './p-list.component.css'
})

/**
* La classe `PListComponent` représente le composant de liste pour les produits (Product).
* Elle gère l'affichage d'une liste d'entités, en utilisant le composant `ListComponent`.
*/
export class PListComponent {
  /**
  * Référence à la liste des produits dans le composant (app-list) utilisé dans le template.
  */
  @ViewChild('list') list!: ListComponent;

  /**
   * Le type de la page associée à la liste des produits.
   */
  nameType = namePageType.Product;

  /**
   * Le nom de l'entité associée à la liste des produits (dans ce cas, "Product").
   */
  entityName = entitiesType.Product;

  /**
   * L'entité de produit à ajouter, initialisée avec une nouvelle instance de produit.
   */
  entity: Product = new Product();

  /**
   * La configuration des champs du formulaire d'ajout de produits.
   */
  config: FormControlConfig[] = [
    { label: 'Nom', controlName: 'name', type: 'text', validators: [Validators.required, Validators.minLength(3), Validators.maxLength(20)] },
    { label: 'Description', controlName: 'description', type: 'text', validators: null },
    {
      label: 'Catégorie', controlName: 'categoryId', type: 'select', validators: [Validators.required],
      options: []
    },
    {
      label: 'DLC (en jour)', controlName: 'durationConservation', type: 'number',
      validators: [Validators.required, Validators.min(1), Validators.max(25), Validators.pattern("^[0-9]*$")]
    }
  ];  

  // injection des dépendances
  categoryService = inject(CategoryService) 

  /**
   * Méthode appelée à l'initialisation du composant.
   */
  ngOnInit() {
    this.onSelectCategory();
  }

  /**
   * Méthode pour récupérer la liste des catégories afin de l'afficher dans le formulaire d'ajout de produits.
   */
  onSelectCategory() {
    this.categoryService.getAll().subscribe(
      (data: Category[]) => {
        const categories = data;
        const categoryControl = this.config.find(c => c.controlName === 'categoryId');
        if (categoryControl) {
          categoryControl.options = categories.map(category => ({ id: category.id, label: category.name }));
        }
      }
    );
  }
}
