import { Injectable, inject } from "@angular/core";
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ConfirmDialogComponent } from "../component/confirm-dialog/confirm-dialog.component";
import { EditorComponent } from "../component/editor/editor.component";

@Injectable({
  providedIn: 'root'
})

/**
 * Le service `DialogService` gère l'affichage des boîtes de dialogue dans l'application.
 */
export class DialogService {
  // injection des dépendances
  dialog = inject(MatDialog) 

  /**
   * Ouvre une boîte de dialogue de confirmation avec le message spécifié.
   * 
   * @param msg - Message à afficher dans la boîte de dialogue.
   * @returns Une référence à la boîte de dialogue ouverte.
   */
  openConfirmDialog(msg: string) {
    return this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      panelClass: 'confirm-dialog-container', // Pour styliser dans style.css
      disableClose: true,
      data: {
        message: msg
      }
    });
  }

  /**
   * Ouvre une boîte de dialogue de formulaire avec les données spécifiées.
   * @param {any} data - Données à transmettre au composant de la boîte de dialogue de formulaire.
   * @returns {MatDialogRef<EditorComponent, any>} - Une référence à la boîte de dialogue ouverte.
   */
  openFormDialog(data: any): MatDialogRef<EditorComponent, any> {  
    return this.dialog.open(EditorComponent, {
      width: '500px',
      height: 'auto',
      panelClass: 'form-dialog-container',
      disableClose: true,
      data: data
    });
  }
}


