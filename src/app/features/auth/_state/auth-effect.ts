import { Injectable, inject } from "@angular/core";
import { Router } from "@angular/router";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Store } from "@ngrx/store";
import { of, throwError } from "rxjs";
import { catchError, exhaustMap, map, mergeMap, tap } from "rxjs/operators";
import { setLoadingSpinner } from "../../../core/_global-state/action/global.action";
import { NotificationService } from "../../../shared/_service/notification.service";
import { RouteService } from "../../../shared/_service/route.service";
import { AuthService } from "../_service/auth.service";
import { LoginAction, ResetPasswordAction, SendMailAction, SignupAction, autoLogin, logout, updateCurrentUser } from "./auth-action";



@Injectable()
/**
* Effets liés à l'authentification, gérant les actions associées aux processus de connexion, d'inscription,
* de réinitialisation de mot de passe, d'autologin et de déconnexion.
*/
export class AuthEffects {
  // injection des dépendances
  actions$ = inject(Actions)
  authService = inject(AuthService)
  routeService = inject(RouteService)
  router = inject(Router)
  notificationService = inject(NotificationService)
  store = inject(Store)

  /**
   * Effet pour le processus de connexion.
   * Lorsque l'action `LoginAction.login_start` est déclenchée, cet effet exécute le processus de connexion
   * et renvoie soit l'action `LoginAction.login_success` en cas de succès, soit l'action `LoginAction.login_fail`
   * en cas d'échec.
   */
  login$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(LoginAction.login_start),
      exhaustMap((action) => {
        return this.authService.login(action.login).pipe(
          map((data) => {
            this.store.dispatch(setLoadingSpinner({ status: false }));
            const currentUser = this.authService.formatUser(data);
            if (!currentUser) {
              this.notificationService.danger('Les informations de l\'utilisateur sont incorrectes');
              return LoginAction.login_fail({ error: 'Invalid user information' });
            }
            this.authService.setUserInLocalStorage(currentUser);
            this.notificationService.success('Connexion réussie');
            return LoginAction.login_success({ currentUser, redirect: true });

          }),
          catchError((error) => {
            this.store.dispatch(setLoadingSpinner({ status: false }));
            this.notificationService.danger("L'identifiant ou le mot de passe est incorrect");
            this.router.navigate(['/auth/login']);
            return throwError(error);
          })
        );
      })
    );
  });

  /**
   * Effet pour l'autologin de l'utilisateur.
   * Lorsque l'action `autoLogin` est déclenchée, cet effet récupère l'utilisateur à partir du stockage local.
   * Si un utilisateur est présent, il envoie l'action `LoginAction.login_success` avec les données de l'utilisateur.
   * Sinon, il envoie l'action `LoginAction.login_fail` avec un message d'erreur approprié.
   */
  autoLogin$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(autoLogin),
      mergeMap(() => {
        const user = this.authService.getUserFromLocalStorage();
        if (user) {
          // Si l'utilisateur est présent, on envoie l'action LOGIN_SUCCESS
          return of(LoginAction.login_success({ currentUser: user, redirect: false }));
        } else {
          // Si l'utilisateur est nul, on envoie l'action LOGIN_FAILURE ou une autre action appropriée
          return of(LoginAction.login_fail({ error: "erreur" }));
        }
      })
    );
  });

  /**
   * Effet pour rediriger vers la page d'accueil après une connexion réussie.
   * Lorsque l'action `LoginAction.login_success` est déclenchée, cet effet redirige l'utilisateur vers la page d'accueil.
   */
  redirectToHome$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(LoginAction.login_success),
      tap((action) => {
        if (action.redirect) {
          this.routeService.goToHome();
        }
      })
    );
  },
    { dispatch: false }
  );

  /**
   * Effet pour le processus d'inscription.
   * Lorsque l'action `SignupAction.signup_start` est déclenchée, cet effet exécute le processus d'inscription
   * et renvoie soit l'action `SignupAction.signup_success` en cas de succès, soit gère les erreurs et redirige
   * l'utilisateur vers la page d'inscription en cas d'échec.
   */
  signUp$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(SignupAction.signup_start),
      exhaustMap((action) => {
        return this.authService.register(action.register).pipe(
          map(() => {
            this.store.dispatch(setLoadingSpinner({ status: false }));
            this.notificationService.success('Enregistrement réussi');
            return SignupAction.signup_success({ register: action.register, redirect: true });
          }),
          catchError(() => {
            this.store.dispatch(setLoadingSpinner({ status: false }));
            this.notificationService.danger("Echec de l'enregistrement");
            this.router.navigate(['/auth/register']);
            return throwError('Une erreur est survenue lors de l\'\enregistrement');
          })
        );
      })
    );
  });

  /**
   * Effet pour rediriger vers la page de connexion après une inscription réussie ou une réinitialisation de mot de passe réussie.
   * Lorsque l'une des actions `SignupAction.signup_success` ou `ResetPasswordAction.reset_password_success` est déclenchée,
   * cet effet redirige l'utilisateur vers la page de connexion.
   */
  redirectToLogin$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(SignupAction.signup_success, ResetPasswordAction.reset_password_success),
      tap((action) => {
        if (action.redirect) {
          this.router.navigate(['/auth/login']);
        }
      })
    );
  },
    { dispatch: false }
  );

  /**
   * Effet pour le processus de déconnexion.
   * Lorsque l'action `logout` est déclenchée, cet effet déconnecte l'utilisateur, nettoie les données de session,
   * et redirige l'utilisateur vers la page de connexion.
   */
  logout$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(logout),
      map(() => {
        this.authService.logout();
        this.router.navigate(['auth']);
      })
    );
  },
    { dispatch: false }
  );

  /**
   * Effet pour le processus de réinitialisation de mot de passe.
   * Lorsque l'action `ResetPasswordAction.reset_password_start` est déclenchée, cet effet exécute le processus de réinitialisation
   * de mot de passe et renvoie l'action `ResetPasswordAction.reset_password_success` en cas de succès, ou gère les erreurs en cas d'échec.
   */
  resetPassword$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(ResetPasswordAction.reset_password_start),
      exhaustMap((action) => {
        return this.authService.resetPassword(action.resetPassword).pipe(
          map(() => {
            this.store.dispatch(setLoadingSpinner({ status: false }));
            this.notificationService.success('Réinitialisation du mot de passe réussie');
            return ResetPasswordAction.reset_password_success({ redirect: true });
          }),
          catchError(() => {
            this.store.dispatch(setLoadingSpinner({ status: false }));
            this.notificationService.danger("Échec de la réinitialisation du mot de passe");
            return throwError('Une erreur est survenue lors de la réinitialisation du mot de passe');
          })
        );
      })
    );
  });

  /**
   * Effet pour le processus d'envoi d'e-mail pour la récupération de mot de passe.
   * Lorsque l'action `SendMailAction.send_mail_start` est déclenchée, cet effet envoie l'e-mail pour la récupération de mot de passe.
   * Il renvoie l'action `SendMailAction.send_mail_success` en cas de succès ou gère les erreurs en cas d'échec.
   */
  sendMail$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(SendMailAction.send_mail_start),
      exhaustMap((action) => {
        return this.authService.sendMail(action.forgotPassword).pipe(
          map(() => {
            this.store.dispatch(setLoadingSpinner({ status: false }));
            return SendMailAction.send_mail_success({ sendMail: true });
          }),
          catchError(() => {
            this.store.dispatch(setLoadingSpinner({ status: false }));
            this.notificationService.danger("L'email est incorrect");
            return of(SendMailAction.send_mail_success({ sendMail: false }));
          })
        );
      })
    );
  });

  /**
   * Effet pour mettre à jour les informations de l'utilisateur actuellement connecté.
   * Cet effet intercepte l'action updateCurrentUser, met à jour les informations de l'utilisateur
   * via le service d'authentification, et ne déclenche pas d'autres actions.
   */
  updateCurrentUser$ = createEffect(() => this.actions$.pipe(
    ofType(updateCurrentUser),
    map(({ currentUser }) => {
      this.authService.updateUser(currentUser);
    })
  ), { dispatch: false });
}
