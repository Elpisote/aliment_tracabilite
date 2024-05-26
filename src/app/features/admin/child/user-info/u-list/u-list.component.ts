import { CommonModule, DatePipe } from '@angular/common';
import { Component, ViewChild, inject } from '@angular/core';
import { FormControlConfig } from '../../../../../core/_utils/FormControlConfig';
import { ListComponent } from '../../../../../shared/component/list/list.component';
import { entitiesType } from '../../../../../shared/enumeration/entities';
import { namePageType } from '../../../../../shared/enumeration/namePage';
import { Role } from '../../_model/role';
import { User } from '../../_model/user';
import { RoleService } from '../../role/role.service';

@Component({
  selector: 'app-u-list',
  standalone: true,
  imports: [
    CommonModule,
    ListComponent
  ],
  providers: [
    RoleService,
    DatePipe
  ],
  templateUrl: './u-list.component.html',
  styleUrl: './u-list.component.css'
})

/**
* La classe `UListComponent` représente le composant de liste pour les utilisateurs (User).
* Elle gère l'affichage d'une liste d'utilisateurs avec des colonnes spécifiques.
*/
export class UListComponent {
  /**
  * Référence à la liste des utilisateurs dans le composant (app-list) utilisé dans le template.
  */
  @ViewChild('list') list!: ListComponent;

  /**
   * Le type de la page associée à la liste des utilisateurs.
   */
  nameType = namePageType.User

  /**
   * Le nom de l'entité associée à la liste des utilisateurs (dans ce cas, "User").
   */
  entityName = entitiesType.User
    
  // L'entité utilisateur à mettre à jour, initialisée à une nouvelle instance de User.
  entity: User = new User();

  // Configuration des contrôles de formulaire pour la mise à jour du rôle.
  config: FormControlConfig[] = [
    { label: 'Nom', controlName: 'lastname', type: 'text', validators: null },
    { label: 'Prénom', controlName: 'firstname', type: 'text', validators: null },
    { label: 'Pseudo', controlName: 'userName', type: 'text', validators: null },
    { label: 'Email', controlName: 'email', type: 'email', validators: null },
    { label: 'Role', controlName: 'role', type: 'select', validators: null, options: [] }
  ];

  // injection des dépendances
  roleService = inject(RoleService)

  // Méthode appelée lors de l'initialisation du composant.
  ngOnInit() {
    this.onSelectRole();
  }

  // Méthode pour récupérer la liste des rôles à afficher dans le formulaire de mise à jour.
  onSelectRole() {   
    this.roleService.getAll().subscribe(
      (data: Role[]) => {
        const roles = data;
        // Met à jour les options du contrôle de sélection des rôles.
        const roleControl = this.config.find(r => r.controlName === 'role');
        if (roleControl) {
          roleControl.options = roles.map(role => ({ id: role.name, label: role.name }));
        }
      }
    );
  }
}
