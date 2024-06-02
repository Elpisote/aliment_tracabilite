import { ApplicationConfig } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { DefaultDataServiceConfig, provideEntityData, withEffects } from '@ngrx/data';
import { provideEffects } from '@ngrx/effects';
import { provideState, provideStore } from '@ngrx/store';
import { routes } from './app.routes';

import { httpInterceptor } from './core/_interceptor/http.interceptor';

import { GlobalReducer } from './core/_global-state/reducer/global.reducer';
import { GLOBAL_STATE_NAME } from './core/_global-state/selector/global.selector';
import { defaultDataServiceConfig } from './core/_utils/data.service.config';
import { entityConfig } from './entity-metadata';
import { AuthService } from './features/auth/_service/auth.service';
import { AuthEffects } from "./features/auth/_state/auth-effect";
import { AuthReducer } from './features/auth/_state/auth-reducer';
import { AUTH_STATE_NAME } from './features/auth/_state/auth-selector';


export const appConfig: ApplicationConfig = {
  providers: [
    AuthService,    
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(withInterceptors([httpInterceptor])),
    provideAnimationsAsync(),
    provideStore(),
    provideState({ name: AUTH_STATE_NAME, reducer: AuthReducer }),
    provideState({ name: GLOBAL_STATE_NAME, reducer: GlobalReducer }),
    provideEffects(AuthEffects),
    provideEntityData(entityConfig, withEffects()),
    { provide: DefaultDataServiceConfig, useValue: defaultDataServiceConfig }, provideAnimationsAsync(), provideAnimationsAsync()
  ]
};
