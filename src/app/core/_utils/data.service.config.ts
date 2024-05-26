import { DefaultDataServiceConfig } from '@ngrx/data';
import { URLBACKEND } from './URL_BACKEND_constant';

// Configuration par défaut pour les services de données
export const defaultDataServiceConfig: DefaultDataServiceConfig = {
  // Définition de l'URL racine pour les appels aux services de données
  root: URLBACKEND.API_BASE_URL
};
