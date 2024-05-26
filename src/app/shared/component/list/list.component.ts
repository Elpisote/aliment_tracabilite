import { CommonModule, DatePipe } from '@angular/common';
import { AfterViewInit, Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { FormGroup, FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormControlConfig } from '../../../core/_utils/FormControlConfig';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Observable, map } from 'rxjs';
import { AuthService } from '../../../features/auth/_service/auth.service';
import { DialogService } from '../../_service/dialog.service';
import { GenericService } from '../../_service/generic.service';
import { NotificationService } from '../../_service/notification.service';
import { entitiesType } from '../../enumeration/entities';
import { namePageType } from '../../enumeration/namePage';
import { EditorData } from '../../interface/IEditorData';
import { StockService } from '../../module/stock/stock.service';
import { SpinnerComponent } from '../spinner/spinner.component';
import { ThemeService } from '../../_service/theme.service';


@Component({
  selector: 'app-list',
  standalone: true,
  imports: [
    CommonModule,
    SpinnerComponent,
    NgxPaginationModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    FormsModule   
  ],
  providers: [
    GenericService,
    AuthService,
    StockService,
    DialogService,
    NotificationService
  ],
  templateUrl: './list.component.html',
  styleUrl: './list.component.css'
})

/**
* Le composant `ListComponent` représente la mise en page d'un affichage tableau de l'application.
* Il gère l'affichage d'une liste d'entités, la recherche, l'ajout, la modification et la suppression d'éléments.
*/
export class ListComponent implements  AfterViewInit {
  // Défini les propriétes input du composant

  /** Tableau de configurations de contrôle de formulaire. */
  @Input() formControlConfigs: FormControlConfig[] = [];

  // Le nom de la page
  @Input() name!: namePageType;

  // Le type d'entité à afficher dans la liste du composant
  @Input() entityName!: entitiesType;

  /** Nom à désactiver dans le contrôle de sélection. */
  @Input() selectNameDisable!: string;

  /** Objet d'entité pour les données du formulaire. */
  @Input() entity!: any;

  // Tableau des noms de colonnes à afficher dans le tableau
  @Input() namecolumns: string[] = [];

  // Tableau de colonnes à afficher dans le tableau, comprenant des noms de propriétés ou des fonctions de formatage
  @Input() columns: (string | ((item: any) => any))[] = [];

  // Indique si le bouton "Ajouter" doit être affiché
  @Input() showPlusButton: boolean = true;

  // Indique si les boutons d'action doivent être affichés
  @Input() showButtonAction: boolean = true;

  // Indique si le composant concerne des stocks (peut être utilisé pour adapter le comportement)
  @Input() isStock: boolean = false;

  // Indique si le composant est de type userInfo. Par défaut c'est false
  @Input() userInfo: boolean = false;

  // Émetteur d'événements pour signaler un changement de page
  @Output() pageChange = new EventEmitter<number>();

  // Formulaire de recherche pour filtrer les éléments de la liste
  searchFormGroup!: FormGroup;

  // Numéro de la page actuellement affichée dans la liste
  page: number = 1;

  // Terme recherche"
  searchTerm: string = '';

  /** Définition du service qui gère les opérations liées à une entité spécifique.
   *Ce service est obtenu à partir du service générique en fonction du type d'entité (par exemple, Product, Category, etc.).
   */
  service: any

  /** Observable utilisé pour suivre l'état de chargement lors du chargement des données depuis le service.
   *Il émet true lorsqu'un chargement est en cours et false une fois le chargement terminé.
   */
  loading$!: Observable<boolean>;

  /** Observable utilisé pour suivre les entités associées à une entité spécifique.
   * Il émet les entités récupérées à partir du service après leur chargement.
   */
  items$!: Observable<any>

  count$!: Observable<number>

  // injection des dépendances
  datePipe = inject(DatePipe)
  genericService = inject(GenericService)
  dialogService = inject(DialogService)
  authService = inject(AuthService)
  stockService = inject(StockService)
  router = inject(Router)
  notificationService = inject(NotificationService)
  themeService = inject(ThemeService);
    
  /**
  * Initialisation du composant.
  * Initialise le formulaire de recherche et récupère la liste des entités.
  */
  ngOnInit(): void {
    // définition du service
    this.service = this.genericService.getService(this.entityName);

    // Initialisation du flux d'éléments
    this.items$ = this.service.entities$
  
    // Appliquer le filtre lors d'une recherche sur le tableau
    this.applyFilter();    
  }

  /**
   * Méthode du cycle de vie Angular appelée après que la vue et les vues enfants
   * sont initialisées. Permet d'exécuter du code après que la vue a été rendue.
   * Dans cette méthode, nous utilisons une Promise pour définir la propriété
   * `loading$` du service sur false, indiquant que le chargement est terminé.
   */
  ngAfterViewInit() {
    Promise.resolve().then(() => this.service.loading$ = false)
  }

  //verifie si la valeur est un string
  isString(value: any): value is string {
    return typeof value === 'string';
  }

  //vérifie si la valeur est une date
  isDate(value: any): value is Date {
    return value instanceof Date || (typeof value === 'string' && !isNaN(Date.parse(value)));
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
    // Accédez aux sous-propriétés de manière récursive
    let value = item;
    for (const prop of properties) {
      value = value[prop];
      if (value === undefined || value === null) {
        break;
      }
      // Si la valeur est un objet, continuez à parcourir les propriétés imbriquées
      if (typeof value === 'object') {
        continue;
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

    if (this.isDate(item[column])) {
      // La colonne est de type Date, appliquer le format de date à l'aide du pipe      
      return this.datePipe.transform(item[column], 'dd/MM/yyyy || HH:mm:ss');
    }
    else if (this.isString(column)) {
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

  /**
   * Obtient l'identifiant d'un élément en utilisant une colonne spécifiée, ou retourne l'identifiant par défaut.
   * @param item L'élément pour lequel récupérer l'identifiant
   * @param column La colonne à utiliser pour l'identifiant, si elle est définie
   * @returns L'identifiant de l'élément ou l'identifiant par défaut (item.id) si la colonne n'est pas définie ou est undefined.
   */
  getItemId(item: any, column: string): any {
    return column && item[column] !== undefined ? item[column] : item.id;
  }

  /**
   * Détermine si un élément doit être désactivé en fonction de certaines conditions.
   * @param item L'élément pour lequel évaluer la désactivation
   * @returns True si l'élément doit être désactivé, sinon False
   */
  shouldDisableItem(item: any): boolean {
    // Les éléments de type User ne sont jamais désactivés
    if (this.entityName === entitiesType.User) {
      return false
    }
    // Désactive l'élément si nbProduct ou nbProductStock n'est pas égal à zéro
    return item.nbProduct !== 0 && item.nbProductStock !== 0;
  }

  /**
   * Applique un filtre de recherche aux éléments de la liste en fonction du terme de recherche spécifié.
   * 
   * Cette méthode filtre les éléments de la liste en fonction du terme de recherche entré par l'utilisateur.
   * Si le terme de recherche est vide ou indéfini, tous les éléments de la liste sont affichés.
   * Le filtre est insensible à la casse et recherche le terme de recherche dans toutes les propriétés
   * des éléments de la liste.
   */
  applyFilter() {
    let searchTerm = this.searchTerm?.trim().toUpperCase();

    if (searchTerm === undefined || searchTerm === '') {
      // Si le terme de recherche est vide, réinitialiser le flux d'éléments
      this.items$ = this.service.entities$;
      return;
    }

    this.items$ = this.service.entities$.pipe(
      map((entities: any) => {
        // Filtrez les entités en fonction du terme de recherche
        return entities.filter((item: any) => {
          // Parcourir toutes les propriétés de l'entité
          for (const column of this.columns) {
            const value = this.formatColumnValue(item, column);
            const formattedValue = value.toString().toUpperCase();
            if (formattedValue.includes(searchTerm)) {
              return true; // Retourner true si le terme de recherche est trouvé dans la propriété
            }
          }
          return false; // Retourner false si le terme de recherche n'est pas trouvé dans toutes les propriétés de l'élément
        });
      })
    );
  }

  /**
   * Redirige l'utilisateur vers la liste des stocks ou vers la page d'ajout,
   * en fonction du rôle de l'utilisateur.
   * Si l'utilisateur a le rôle 'user', il est redirigé vers la page d'ajout de stock pour les utilisateurs.
   * Sinon, s'il a le rôle 'admin', il est redirigé vers la page d'ajout de stock pour les administrateurs.
   */
  goToAddStock(): void {
    // Obtient le rôle actuel de l'utilisateur
    const userRole = this.authService.getCurrentRole();

    // Détermine la route de redirection en fonction du rôle de l'utilisateur
    const redirectRoute = (userRole === 'Admin') ? 'admin/stock/add' : 'user/stock/add';

    // Redirige l'utilisateur vers la route déterminée
    this.router.navigate([redirectRoute]);
  }

  /**
   * Ouvre une boîte de dialogue pour ajouter ou mettre à jour une entité.
   * @param {boolean} isAdd - Un indicateur booléen indiquant si la boîte de dialogue est utilisée pour ajouter une nouvelle entité (true) ou mettre à jour une entité existante (false).
   * @param {any} itemId - L'identifiant de l'entité à mettre à jour. Facultatif, utilisé uniquement lors de la mise à jour d'une entité existante.
   */
  openDialog(isAdd: boolean, item?: any): void {
   
    if (this.entityName === entitiesType.Stock && isAdd) {
      this.goToAddStock();
    } else {
      // Définir l'objet de base sans itemId
      let data: EditorData = {
        formControlConfigs: this.formControlConfigs,
        entity: this.entity,
        entityName: this.entityName,
        isAdd: isAdd,
        userInfo: this.userInfo,
        selectNameDisable: this.selectNameDisable,
      };
           
      // Si c'est une mise à jour (isAdd est faux) et itemId est défini, ajoutez itemId à l'objet data
      if (!isAdd && item !== undefined) {    
        data = { ...data, itemId: item }; // Ajoute l'identifiant de l'entité à mettre à jour à l'objet data
      }  
      // Ouvre la boîte de dialogue pour ajouter ou mettre à jour une entité
      this.dialogService.openFormDialog(data);  
    }
  }

  /**
   * Supprime l'élément spécifié après confirmation de l'utilisateur.
   * @param item L'élément à supprimer
   */
  onDeleteItem(item: any) {   

    // Ouvre une boîte de dialogue de confirmation
    this.dialogService.openConfirmDialog('Etes-vous sûr de vouloir supprimer ?')
      .afterClosed().subscribe(resultat => {
        if (resultat) {
          // Supprime l'entité en appelant le service générique
          this.service.delete(item).subscribe(() => {
            // Affiche une notification de suppression réussie
            if (this.userInfo) {
              this.notificationService.info("L'utilisateur " + item.userName + ' a été supprimé(e) avec succès')
            } else {
              this.notificationService.info(item.name + ' supprimé(e) avec succès');
            }
          })          
        }
      })
  }  
}
