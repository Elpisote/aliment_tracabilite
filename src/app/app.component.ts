import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { getLoading } from './core/_global-state/selector/global.selector';
import { autoLogin } from './features/auth/_state/auth-action';
import { SpinnerComponent } from './shared/component/spinner/spinner.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CommonModule,
    SpinnerComponent
  ],
  providers: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'ANG_resto';

  showLoading!: Observable<boolean>;

  // injection des dépendances
  store = inject(Store)
 
  ngOnInit() {
    this.showLoading = this.store.select(getLoading);
    this.store.dispatch(autoLogin());   
  } 
}
