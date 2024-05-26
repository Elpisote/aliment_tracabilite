import { CommonModule, DatePipe, TitleCasePipe } from '@angular/common';
import { Component, Inject, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { FormControlConfig } from '../../../core/_utils/FormControlConfig';
import { ErrorService } from '../../_service/errorForms.service';
import { GenericService } from '../../_service/generic.service';
import { NotificationService } from '../../_service/notification.service';
import { entitiesType } from '../../enumeration/entities';
import { SpinnerComponent } from '../spinner/spinner.component';


@Component({
  selector: 'app-editor',
  standalone: true,
  imports: [
    CommonModule,
    SpinnerComponent,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatDialogModule
  ],
  providers: [
    TitleCasePipe,
    DatePipe,
    ErrorService,
    NotificationService,
    GenericService
  ],
  templateUrl: './editor.component.html',
  styleUrl: './editor.component.css'
})

/**
* La classe `EditorComponent` représente le composant générique
* pour gérer l'édition et la création de formulaire
*/
export class EditorComponent {
  /** Tableau de configurations de contrôle de formulaire. */
  formControlConfigs!: FormControlConfig[];

  /** Nom de l'entité pour la manipulation du formulaire. */
  entityName!: entitiesType;

  /** Nom à désactiver dans le contrôle de sélection. */
  selectNameDisable!: string;

  /** Objet d'entité pour les données du formulaire. */
  entity!: any;

  /** Drapeau pour déterminer s'il s'agit d'une opération d'ajout ou de mise à jour. */
  isAdd: boolean = true; // si ce n'est pas une ajout, c'est une mise à jour

  /**Boolean pour vérifier si on est dans user info **/
  userInfo: boolean = false;

  /** Groupe de formulaires pour gérer les contrôles du formulaire. */
  formGroup!: FormGroup;

  /** Identifiant de l'entité. */
  id!: number;

  /** 
   * Objet pour stocker les données du formulaire.
   * Il est initialisé ultérieurement et peut contenir n'importe quel type de valeur.
   */
  item!: any;

  /**  Service utilisé pour effectuer des opérations spécifiques sur les données
   *  liées à l'entité en cours.
   */
  service!: any   

  /**
   * Indique si une opération de chargement est en cours.
   * Si true, une opération de chargement est en cours ; sinon, false.
   */
  loading!: boolean;


  // injection des dépendances
  fb = inject(FormBuilder)
  errorService = inject(ErrorService)
  notificationService =inject(NotificationService)
  titlecasePipe = inject(TitleCasePipe)
  datePipe = inject(DatePipe)
  genericService = inject(GenericService)  

  constructor(@Inject(MAT_DIALOG_DATA) public formData: any) {
    // Initialisation des propriétés de l'éditeur avec les données fournies
    this.formControlConfigs = formData.formControlConfigs;
    this.entity = formData.entity
    this.entityName = formData.entityName
    this.isAdd = formData.isAdd
    this.userInfo = formData.userInfo
    this.selectNameDisable = formData.selectNameDisable
    this.service = this.genericService.getService(this.entityName);   
  }

  /**
  * Méthode appelée à l'initialisation du composant
  * Initialisation du formulaire.
  */
  ngOnInit() {
    this.initializeForm();    
  }

  /**
   * Vérifie si une valeur est une date.
   * @method
   * @param {any} value - La valeur à vérifier.
   * @returns {boolean} - True s'il s'agit d'une date, false sinon.
   */
  isDate(value: any): value is Date {
    return value instanceof Date || (typeof value === 'string' && !isNaN(Date.parse(value)));
  }

  /**
   * Obtient la valeur d'une propriété imbriquée à partir d'un objet.
   * @method
   * @param {any} item - L'objet pour obtenir la valeur de la propriété.
   * @param {string} propertyPath - Le chemin de la propriété.
   * @returns {any} - La valeur de la propriété.
   */
  getItemProperty(item: any, propertyPath: string): any {
    // Divisez la chaîne des propriétés en tableau pour accéder aux sous-propriétés
   
    const properties = propertyPath.split('_');
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
   * Formate la valeur d'un contrôle de formulaire.
   * @method
   * @param {string} controlName - Le nom du contrôle de formulaire.
   * @returns {any} - La valeur formatée.
   */
  formatColumnValue(controlName: string): any {
    let value;
 
    if (controlName.includes('_')) {
      value = this.getItemProperty(this.entity, controlName);
    } else {
      value = this.formGroup.get(controlName)?.value;
    }

    // Trouver la configuration de contrôle correspondant à controlName
    const config = this.formControlConfigs.find(c => c.controlName === controlName);
 
    // Si la configuration n'est pas trouvée ou si le type est 'number', retourner la valeur telle quelle
    if (!config || config.type === 'number') {
      return value;
    }

    // Vérifier si la valeur est une date
    if (this.isDate(value)) {
      return this.datePipe.transform(value, 'dd/MM/yyyy || HH:mm:ss');
    }
    // Si ce n'est ni un nombre ni une date, retourner la valeur telle quelle
    return value;
  }

  /**
   * Initialise le formulaire de l'éditeur.
   * Si l'opération n'est pas une création (isAdd est faux), récupère les données de l'entité par son identifiant.
   */ 
  private initializeForm() {
    // Configuration du formulaire
    const formGroupConfig: { [key: string]: any } = {};

    // Parcours des configurations de contrôle
    this.formControlConfigs.forEach(config => {
      formGroupConfig[config.controlName] = [null, config.validators];
    });

    // Si ce n'est pas une opération d'ajout, récupération des données de l'entité par son identifiant
    if (!this.isAdd) {
      // Création du groupe de formulaires avec la configuration
      this.formGroup = this.fb.group(formGroupConfig);

      // chargement en cours
      this.loading = true 
      // Appel du service pour obtenir les données de l'entité
        this.service.getByKey(this.formData.itemId.id).subscribe({
          next: (data: any) => { 
            //chargement fini
            this.loading = false
            // Affectation des données de l'entité aux champs du formulaire
            this.entity = data;

            this.formControlConfigs.forEach(config => {
              let controlValue;
              if (config.controlName.includes('_')) {
                controlValue = this.getItemProperty(this.entity, config.controlName);
              } else {
                controlValue = this.entity[config.controlName];
              }
              formGroupConfig[config.controlName] = [controlValue, config.validators];
            });
            this.formGroup = this.fb.group(formGroupConfig);
        },
        error: (error: any) => {
          console.error('Error fetching entity', error);
        }
      });
    } else {
      // Si c'est une opération d'ajout, création du groupe de formulaires avec la configuration
      this.formGroup = this.fb.group(formGroupConfig);
    }
  }    

  /**
   * Obtient le message d'erreur pour un champ de formulaire donné.
   * @method
   * @param {string} fieldName - Le nom du champ du formulaire.
   * @param {string} field - Le nom du champ du formulaire.
   * @returns {string} - Le message d'erreur.
   */
  getMessageErreur(fieldName: string, field: string): string {
    const errors = this.formGroup.controls[field].errors;
    // Vérifiez si les erreurs sont nulles
    if (errors) {
      return this.errorService.getErrorMessage(fieldName, errors);
    }
    // Si les erreurs sont nulles, retournez une chaîne vide ou un message par défaut selon vos besoins.
    return '';
  }

  /**
   * Vérifie si un contrôle de formulaire est en mode lecture seule.
   * @method
   * @param {string} controlName - Le nom du contrôle de formulaire.
   * @returns {boolean} - True si en lecture seule, false sinon.
   */
  isReadonly(controlName: string): boolean {
    const isSelectControl = this.formControlConfigs.find(config => config.controlName === controlName)?.type === 'select';

    return (
      (this.entityName === entitiesType.User ||
        this.entityName === entitiesType.Stock) && !isSelectControl
    );
  }

  /**
   * Gère l'événement de soumission du formulaire.
   * Si le formulaire est valide, appelle la méthode de création ou de mise à jour en fonction de l'opération.
   * Si la route est de type Category ou Product, formate la valeur du champ 'name' en majuscule pour la première lettre.
   */
  onSubmit() {
    // Vérification de la validité du formulaire
    if (this.formGroup.valid) {
      // Si l'opération est une création, appelle la méthode de création, sinon appelle la méthode de mise à jour
      if (this.isAdd) {
        this.createItem();
      } else {     
        this.updateItem();
      }
    }
  }

  /**
   * Crée une nouvelle entité à partir des valeurs du formulaire et l'ajoute via le service générique.
   * Affiche un message de succès, puis redirige vers la liste des entités correspondantes.
   */
  createItem() {
    // Création de l'entité à partir des valeurs du formulaire
    let newItem = { ...this.entity, ...this.formGroup.value };

    // Utilisation de la méthode pour capitaliser la première lettre si nécessaire
    newItem = this.capitalizeFirstLetter(newItem, this.formGroup, this.entityName);

    this.service.add(newItem).subscribe(
      this.notificationService.success(newItem.name + ' ajouté(e) avec succès')    
    );
  }

  /**
   * Met à jour l'entité existante avec les valeurs du formulaire via le service générique.
   * Affiche un message de succès, puis redirige vers la liste des entités correspondantes.
   */
  updateItem() {
    let item = this.capitalizeFirstLetter({ ...this.entity, ...this.formGroup.value }, this.formGroup, this.entityName);
       
    this.service.update(item).subscribe(
      () => {        
        if (this.userInfo) {
          this.notificationService.success("L'utilisateur " + item.userName + ' mis à jour avec succès');
        } else {
          this.notificationService.success(item.name + ' mis(e) à jour avec succès');
        }
      },
      (error: any) => {       
        console.error('Erreur lors de la mise à jour de l\'élément ' + this.entityName + ' :', error);
      }
    );    
  }

  /**
   * Capitalise la première lettre de la valeur du champ 'name' dans l'objet passé en paramètre, si l'entité correspondante est de type Category ou Product.
   * @param item - L'objet dont la première lettre du champ 'name' doit être capitalisée.
   * @param formGroup - Le FormGroup contenant le champ 'name'.
   * @param entityName - Le nom de l'entité associée à l'objet.
   * @returns L'objet modifié avec la première lettre du champ 'name' capitalisée, ou l'objet d'origine si l'entité n'est pas de type Category ou Product.
   */
  capitalizeFirstLetter(item: any, formGroup: FormGroup, entityName: string): any {
    // Vérifie si l'entité correspondante est de type Category ou Product
    if (entityName == entitiesType.Category || entityName == entitiesType.Product) {
      // Récupère la valeur du champ 'name' du formulaire
      let format = formGroup.get('name')?.value;
      // Transforme la première lettre en majuscule à l'aide du pipe 'titlecase'
      format = this.titlecasePipe.transform(format);
      // Crée une copie de l'objet original pour éviter les modifications directes
      item = { ...item };
      // Met à jour la valeur du champ 'name' dans l'objet copié avec la première lettre capitalisée
      item.name = format;
    }
    // Retourne l'objet modifié ou l'objet d'origine si l'entité n'est pas de type Category ou Product
    return item;
  }
}
