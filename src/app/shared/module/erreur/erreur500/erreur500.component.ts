import { Component } from '@angular/core';
import { ErrorComponent } from '../../../component/error/error.component';

@Component({
  selector: 'app-erreur500',
  standalone: true,
  imports: [ErrorComponent],
  templateUrl: './erreur500.component.html',
  styleUrl: './erreur500.component.css'
})
export class Erreur500Component {

}
