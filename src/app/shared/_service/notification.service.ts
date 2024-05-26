import { Injectable, inject } from "@angular/core";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material/snack-bar";
import { SnackbarComponent } from "../component/snackbar/snackbar.component";

@Injectable({
  providedIn: 'root'
})

// Service pour afficher des notifications lors d'actions spécifiques
export class NotificationService {

  // injection des dépendances
  snackBar = inject(MatSnackBar)

  // Configuration par défaut de la snackbar
  config: MatSnackBarConfig = {
    duration: 3000,
    horizontalPosition: 'center',
    verticalPosition: 'bottom',
  }

  // Autre configuration de la snackbar
  config2: MatSnackBarConfig = {
    horizontalPosition: 'center',
    verticalPosition: 'top'
  }

  /**
   * Affiche une notification de succès.
   * @param msg Le message à afficher.
   */
  success(message: string) {
    this.config['panelClass'] = ['notif-success'];
    this.snackBar.openFromComponent(SnackbarComponent, {
      ...this.config, 
      data: {
        message: message,
        icon: 'done',
        snackBar: this.snackBar
      }
    })
  }

  /**
   * Affiche une notification de danger.
   * @param msg Le message à afficher.
   */
  danger(message: string) {
    this.config2['panelClass'] = ['notif-danger'];
    this.snackBar.openFromComponent(SnackbarComponent, {
      ...this.config2,
      data: {
        message: message,     
        icon: 'report_problem',
        snackBar: this.snackBar
      }
    })
  }

  /**
   * Affiche une notification d'avertissement.
   * @param msg Le message à afficher.
   */
  info(message: string) {   
    this.config['panelClass'] = ['notif-info'];
    this.snackBar.openFromComponent(SnackbarComponent, {
      ...this.config,
      data: {
        message: message,
        icon: 'info',
        snackBar: this.snackBar
      }
    })
  }
}

