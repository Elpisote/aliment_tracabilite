import { CommonModule } from '@angular/common';
import { Component, Input, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { Observable, of, switchMap } from 'rxjs';
import { User } from '../../../../features/admin/child/_model/user';
import { UserService } from '../../../../features/admin/child/user-info/user.service';
import { RouteService } from '../../../_service/route.service';

@Component({
  selector: 'app-i-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatGridListModule,
    MatButtonModule
  ],
  providers: [
    UserService,
    RouteService
  ],
  templateUrl: './i-list.component.html',
  styleUrl: './i-list.component.css'
})

/**
  * La classe `IListComponent` représente un composant Angular destiné
  * à afficher une liste d'informations sur l'utilisateur   * 
  */
export class IListComponent {
  //Indique l'état d'affichage des informations. Par défaut, il est défini sur `false`.
  @Input() infoStatus: boolean = false;

  //Contient l'identifiant de l'utilisateur actuel.
  currentUsername!: string
 
  //Contient les détails de l'utilisateur actuel.
  user$!: Observable<User>;

  // injection des dépendances
  userService = inject(UserService)
  routeService = inject(RouteService)


  /**
   * Méthode appelée lors de l'initialisation du composant.
   * Elle récupère les informations de l'utilisateur actuel.   
   */
  ngOnInit(): void {
    this.currentUsername = localStorage.getItem('username') as string;
    if (this.currentUsername) {
      this.userService.getUserIdByUsername(this.currentUsername).pipe(
        switchMap((id: string | undefined) => {
          if (id) {
            return this.userService.getByKey(id);
          } else {
            // Gérez le cas où l'ID n'est pas trouvé
            return of(null);
          }
        })
      ).subscribe((user: User | null) => {
        if (user) {
          this.user$ = of(user);
        } else {
          // Gérez le cas où l'utilisateur n'est pas trouvé
        }
      });
    } else {
      //voir pour se deconnecter peut être
    }   
  }

  //Redirige l'utilisateur vers la page d'accueil appropriée à la fermeture du composant.  
  onClose() {
    this.routeService.goToHome();
  }

  //Redirige l'utilisateur vers la page appropriée pour la modification des informations.
  onUpdateInformation(id: string) {
    this.routeService.goToInformation(id, true)
  }
}
