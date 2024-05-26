import { Injectable } from "@angular/core";
import { EntityCollectionServiceBase, EntityCollectionServiceElementsFactory } from "@ngrx/data";
import { Observable, catchError, map, of, tap } from "rxjs";
import { entitiesType } from "../../../../shared/enumeration/entities";
import { User } from "../_model/user";

@Injectable({
  providedIn: 'root',
})

/**
  * Service de gestion des utilisateurs.
  * 
  * Cette classe étend EntityCollectionServiceBase<User>, ce qui signifie qu'elle fournit des fonctionnalités pour la manipulation
  * d'une collection d'entités de type User.
  */
export class UserService extends EntityCollectionServiceBase<User> {
  /**
  * Constructeur de la classe CategoryService.
  * 
  * @param http Un objet de type HttpClient utilisé pour effectuer des requêtes HTTP.
  * @param serviceElementsFactory Un objet de type EntityCollectionServiceElementsFactory utilisé pour la création des services.
  */
  constructor(serviceElementsFactory: EntityCollectionServiceElementsFactory) {
    super(entitiesType.User, serviceElementsFactory);
  }

  getUserIdByUsername(username: string): Observable<string | undefined> {
    return this.getWithQuery({ username }).pipe(
      map(users => {
        const user = users.find(u => u.userName === username); // Trouver l'utilisateur correspondant au nom d'utilisateur
        return user ? user.id : undefined; // Retourner l'ID de l'utilisateur s'il est trouvé, sinon undefined
      }),
      catchError(error => {
        console.error('Une erreur s\'est produite lors de la récupération de l\'utilisateur par nom d\'utilisateur:', error);
        return of(undefined); // Retourner undefined en cas d'erreur
      })
    );
  }

  simpleTest(): void {
    console.log('UserService is working correctly');
  }

  refreshUsers() {
    console.log('refreshUsers called');
    this.getAll().subscribe(users => {
      console.log(users); // Les données réelles sont loguées ici
    });
  }
}
