import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { CommonModule } from '@angular/common';
import { Component, ViewChild, inject } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSelect, MatSelectModule } from '@angular/material/select';
import { MatStepper, MatStepperModule } from '@angular/material/stepper';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { NgxPrintModule } from 'ngx-print';
import { forkJoin } from 'rxjs';
import { Category } from '../../../../features/admin/child/_model/category';
import { Product } from '../../../../features/admin/child/_model/product';
import { CategoryService } from '../../../../features/admin/child/category/category.service';
import { ProductService } from '../../../../features/admin/child/product/product.service';
import { AuthService } from '../../../../features/auth/_service/auth.service';
import { NotificationService } from '../../../_service/notification.service';
import { ProductListSelectionComponent } from '../../../component/product-list-selection/product-list-selection.component';
import { StockService } from '../stock.service';


@Component({
  selector: 'app-s-add',
  standalone: true,
  imports: [
    CommonModule,
    ProductListSelectionComponent,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatStepperModule,
    MatFormFieldModule,
    MatPaginatorModule,
    NgxPrintModule
  ],
  providers: [
    ProductService,
    StockService
  ],
  templateUrl: './s-add.component.html',
  styleUrl: './s-add.component.css'
})

/**
* Composant Angular pour la gestion de l'ajout de stocks.
* Permet la sélection de produits, la vérification et l'impression des stocks ajoutés.
*/
export class SAddComponent {
  /** Références aux éléments de pagination, d'étapes et de sélection de catégories */
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatStepper) stepper!: MatStepper;
  @ViewChild('categoriesSelect') categoriesSelect!: MatSelect;
  @ViewChild(ProductListSelectionComponent) productListSelectionComponent!: ProductListSelectionComponent;

  /** Liste des catégories récupérées */
  categories!: Array<Category>;

  /** Catégories sélectionnées */
  selectedCategories!: any;

  /** Indicateur de validation désactivée */
  validateDisabled = false;

  /** Indicateur d'étape 1 complétée */
  step1Completed = false;

  /** Contrôles de filtre pour l'ID, le nom et la DLC des produits */
  idFilter = new FormControl();
  nameFilter = new FormControl();
  dlcFilter = new FormControl();
  categoryFilter = new FormControl();

  /** Source de données pour le tableau de produits */
  dataSource = new MatTableDataSource<Product>([]);

  /** Source de données pour le tableau de produits sélectionnés */
  dataSourceSelected = new MatTableDataSource<Product>([]);

  /** Valeurs de filtrage */
  filterValues = {
    id: '',
    name: '',
    durationConservation: ''
  };

  // injection des dépendances
  productService = inject(ProductService)
  categoryService = inject(CategoryService)
  stockService = inject(StockService)
  authService = inject(AuthService)
  notificationService = inject(NotificationService)
  router = inject(Router)


  /**
   * Méthode pour récupérer tous les produits.
   * Met à jour la source de données du tableau.
   */
  getAllProducts() {
    this.productService.getAll().subscribe({
      next: (data: any) => {
        this.dataSource.data = data
      }
    });
  }

  /**
   * Méthode pour récupérer toutes les catégories.
   * Met à jour la liste des catégories.
   */
  getAllCategories() {   
    this.categoryService.getAll().subscribe({
      next: (data: any) => {
        // Assigne les données récupérées à la propriété items
        this.categories = data;
      }
    });
  }
  
  constructor() {
    this.getAllProducts();
    this.getAllCategories();
    this.dataSource.filterPredicate = this.createFilter();
  }

  /**
 * Méthode appelée lors de l'initialisation du composant.
 * Configure les abonnements aux modifications des filtres de recherche.
 */
  ngOnInit() {
    this.onCategorySelectionChange();
    this.nameFilter.valueChanges
      .subscribe(
        name => {
          this.filterValues.name = name;
          this.dataSource.filter = JSON.stringify(this.filterValues);
        }
      )
    this.idFilter.valueChanges
      .subscribe(
        id => {
          this.filterValues.id = id;
          this.dataSource.filter = JSON.stringify(this.filterValues);
        }
      )
    this.dlcFilter.valueChanges
      .subscribe(
        dlc => {
          this.filterValues.durationConservation = dlc;
          this.dataSource.filter = JSON.stringify(this.filterValues);
        }
      )
  }

  /**
 * Méthode appelée après l'initialisation des vues enfants.
 * Associe le paginator à la source de données du tableau.
 */
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  /**
 * Méthode appelée lors du changement de sélection des catégories.
 * Récupère les produits correspondant aux catégories sélectionnées.
 */
  onCategorySelectionChange() {
    this.categoryFilter.valueChanges.subscribe(selectedIds => {
      // Si aucun ID de catégorie n'est sélectionné, n'effectuez pas de requête
      if (!selectedIds || selectedIds.length === 0) {
        this.getAllProducts()
        return; // Sortir de la méthode
      }

      // Appeler le service pour récupérer les produits correspondant aux catégories sélectionnées
      this.productService.getProductByCategories(selectedIds).subscribe((data: Product[]) => {
        // Mettre à jour la source de données du tableau sans créer une nouvelle instance de MatTableDataSource
        this.dataSource.data = data;
      });
    });
  }

  /**
 * Crée une fonction de prédicat de filtrage pour la source de données du tableau.
 * @returns La fonction de prédicat de filtrage
 */
  createFilter(): (data: any, filter: string) => boolean {
    let filterFunction = function (data: {
      name: string;
      id: { toString: () => string; };
      durationConservation: { toString: () => string; }; // Utilisez durationConservation au lieu de dlc
    }, filter: string): boolean {
      let searchTerms = JSON.parse(filter);
      let idString = data.id ? data.id.toString().toLowerCase() : '';
      let durationConservationString = data.durationConservation ? data.durationConservation.toString().toLowerCase() : ''; // Utilisez durationConservationString au lieu de dlcString
      return data.name.toLowerCase().indexOf(searchTerms.name) !== -1
        && idString.indexOf(searchTerms.id) !== -1
        && durationConservationString.indexOf(searchTerms.durationConservation) !== -1 // Utilisez durationConservationString au lieu de dlcString
    }
    return filterFunction;
  }


  /**
 * Méthode appelée lors du changement de sélection d'éléments.
 * Récupère les détails des éléments sélectionnés et met à jour la liste des produits sélectionnés.
 */
  validation() {
    if (this.productListSelectionComponent) {
      const selectedItems = this.productListSelectionComponent.selection.selected;
      if (selectedItems.length === 0) {
        this.step1Completed = false;
      } else {
        this.step1Completed = true;
      }
      const requests = selectedItems.map((s: { id: any }) => {       
        return this.productService.getByKey(s.id);
      });
      forkJoin(requests).subscribe((data: any) => {
        this.dataSourceSelected.data = data;
      });
    } else {
      console.log('StockListComponent is not initialized');
    }
  }

  /**
 * Récupère la date et l'heure actuelles au format local.
 * @returns La date et l'heure actuelles au format local
 */
  getCurrentDate(): string {
    const currentDate = new Date();
    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'short', day: '2-digit', hour: '2-digit', minute: '2-digit' };
    return currentDate.toLocaleString('fr-FR', options).replace(',', ' ||');
  }

  /**
 * Calcule la date de péremption en fonction de la durée de conservation.
 * @param dureeConservation La durée de conservation en jours
 * @returns La date de péremption calculée
 */
  getDLC(dureeConservation: number): string {
    const currentDate = new Date();
    const dateDePeremption = new Date(currentDate.getTime() + dureeConservation * 24 * 60 * 60 * 1000);
    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'short', day: '2-digit', hour: '2-digit', minute: '2-digit' };
    return dateDePeremption.toLocaleString('fr-FR', options).replace(',', ' ||');
  }

  /**
 * Méthode appelée lors du changement de sélection dans le stepper.
 * @param event L'événement de sélection du stepper
 */
  selectionChange(event: StepperSelectionEvent) { }

  /**
 * Crée les stocks pour les produits sélectionnés.
 * Appelle le service pour créer les stocks avec les IDs des produits sélectionnés.
 */
  createStocks() {
    const productIds = this.dataSourceSelected.data.map((s: { id: any }) => s.id);
    this.stockService.addStocks(productIds).subscribe({
      next: () => { },
    })
  }

  /**
 * Lance l'impression des stocks ajoutés.
 * Appelle la méthode pour créer les stocks puis lance l'impression.
 */
  printing() {
    this.createStocks();
    this.notificationService.success('Produit(s) en stock ajouté(s) avec succès')
    this.goToStockList();
  }

  /**
  * Redirige l'utilisateur vers la liste des stocks en fonction de son rôle.
  * Si l'utilisateur a le rôle 'user', il est redirigé vers la liste des stocks pour les utilisateurs.
  * Sinon, s'il a le rôle 'admin', il est redirigé vers la liste des stocks pour les administrateurs.
  */
  goToStockList(): void {
    // Obtient le rôle actuel de l'utilisateur
    const userRole = this.authService.getCurrentRole();

    // Détermine la route de redirection en fonction du rôle de l'utilisateur
    const redirectRoute = (userRole === 'Admin') ? 'admin/stock' : 'user/stock';

    // Redirige l'utilisateur vers la route déterminée
    this.router.navigate([redirectRoute]);
  }
}
