import { HttpErrorResponse, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { apiToken } from '../../features/auth/_models/apiToken';
import { AuthService } from '../../features/auth/_service/auth.service';
import { NotificationService } from '../../shared/_service/notification.service';

/**
 * Intercepte les requêtes sortantes, ajoute le jeton d'authentification
 * s'il est présent, et gère les erreurs d'authentification.
 *
 * @param request Requête HTTP sortante.
 * @param next Handler HTTP pour la requête suivante.
 * @returns Observable d'événements HTTP.
 */
export const httpInterceptor: HttpInterceptorFn = (request, next) => {
  const router = inject(Router)
  const notificationService = inject(NotificationService)
  const authService = inject(AuthService)
  const token = authService.getToken();

  if (token) { 
    request = addTokenHeader(request, token);
  }
 
  return next(request).pipe(
    catchError((err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401 && token !== null && !request.url.includes("login")) {
          return handleUnAuthorizedError(request, next);
        }
        else if (err.status === 401 && request.url.includes("login")) {
          notificationService.danger("L'identifiant ou le mot de passe est incorrect")
          router.navigate(['/auth/login']);
        }
        else {
          const errorMessage = handleError(err);
          notificationService.danger(errorMessage)
          return throwError(errorMessage);
        }
      }
      return throwError(() => err)
    })
  );
};

/**
 * Ajoute un en-tête d'autorisation contenant le jeton à la requête HTTP.
 *
 * @param request Requête HTTP.
 * @param token Jeton d'authentification.
 * @returns Requête HTTP modifiée.
 */
function addTokenHeader(request: HttpRequest<any>, token: string): HttpRequest<any> {
  return request.clone({ headers: request.headers.set('Authorization', 'Bearer ' + token) });
}

/**
   * Gère les erreurs d'authentification 401 en rafraîchissant le jeton.
   *
   * @param req Requête HTTP.
   * @param next Handler HTTP pour la requête suivante.
   * @returns Observable d'événements HTTP.
   */
function handleUnAuthorizedError(req: HttpRequest<any>, next: HttpHandlerFn) {
  const router = inject(Router)
  const authService = inject(AuthService)

  const aToken = localStorage.getItem('token') as string
  const rToken = localStorage.getItem('refreshToken') as string

  let apiT = new apiToken()
  apiT.accessToken = aToken
  apiT.refreshToken = rToken;

  return authService.newToken(apiT)
    .pipe(
      switchMap((data: apiToken) => {
        localStorage.setItem('token', data.accessToken);
        localStorage.setItem('refreshToken', data.refreshToken);       
        req = addTokenHeader(req, data.accessToken)
        return next(req);
      }),
      catchError((err) => {
        return throwError(() => {
          router.navigate(['auth/login'])
        })
      })
    )
}

/**
   * Gère les erreurs HTTP en fonction du statut de la réponse.
   *
   * @param error Erreur HTTP.
   * @returns Message d'erreur approprié.
   */
function handleError(error: HttpErrorResponse): string {
  const router = inject(Router)
  let errorMessage = error.error.message

  if (error instanceof HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      console.log(`error status : ${error.status} ${error.statusText}`);
      switch (error.status) {
        case 403:     //forbidden
          errorMessage = "accès interdit"
          router.navigate(['/erreur/erreur403']);
          break;
        case 404:      //Not found
          errorMessage = "Erreur dans le service. " + error.error.message + ". Veuillez contacter un administrateur"
          break;
        case 500:     //servor error
          errorMessage = "Erreur serveur"
          router.navigate(['/erreur/erreur500']);
          break;
      }
    }
  } else {
    console.error("some thing else happened");
    errorMessage = 'Veuillez contacter un administrateur'
    router.navigate(['/erreur/error']);
  }
  return errorMessage
}
