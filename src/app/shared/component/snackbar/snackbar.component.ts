import { Component, Inject, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_SNACK_BAR_DATA, MatSnackBar } from '@angular/material/snack-bar';
import { NotificationService } from '../../_service/notification.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-snackbar',
  standalone: true,
  imports: [MatButtonModule, MatIconModule],
  templateUrl: './snackbar.component.html',
  styleUrl: './snackbar.component.css'
})
export class SnackbarComponent {
  snackbar = inject(MatSnackBar)
  notificationService = inject(NotificationService)

  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: any) { }

  onClose() {
    this.data.snackBar.dismiss();
  }
}
