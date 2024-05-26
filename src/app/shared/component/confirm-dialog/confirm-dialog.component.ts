import { CommonModule } from '@angular/common';
import { Component, Inject, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule
  ],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.css'
})

/**
* Composant Angular représentant une boîte de dialogue de confirmation.
*/
export class ConfirmDialogComponent {
  // injection des dépendances
  dialogRef = inject(MatDialogRef<ConfirmDialogComponent>)

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) { }

  /**
   * Méthode appelée pour fermer la boîte de dialogue avec la valeur false.
   */
  closeDialog() {
    this.dialogRef.close(false);
  }
}
