import { Component } from '@angular/core';
import { ErrorComponent } from '../../../component/error/error.component';

@Component({
  selector: 'app-erreur-globale',
  standalone: true,
  imports: [ErrorComponent],
  templateUrl: './erreur-globale.component.html',
  styleUrl: './erreur-globale.component.css'
})
export class ErreurGlobaleComponent {

}
