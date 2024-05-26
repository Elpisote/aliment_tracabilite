import { createAction, props } from '@ngrx/store';

// Définition d'une constante représentant le type d'action
export const SET_LOADING_ACTION = '[global state] set loading spinner';

// Création de l'action pour définir l'état du spinner de chargement
export const setLoadingSpinner = createAction(
  // Utilisation du type d'action défini
  SET_LOADING_ACTION,
  // Définition des propriétés de l'action avec l'utilisation de props
  props<{ status: boolean }>() // L'action prend un booléen 'status' indiquant l'état du chargement
);

