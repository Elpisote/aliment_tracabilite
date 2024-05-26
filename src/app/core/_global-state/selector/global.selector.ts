import { createFeatureSelector, createSelector } from "@ngrx/store";
import { GlobalState } from "../state/global.state";


// Nom de l'état global utilisé pour la sélection des fonctionnalités
export const GLOBAL_STATE_NAME = 'global';

// Sélecteur pour obtenir l'état global
const getGlobalState = createFeatureSelector<GlobalState>(GLOBAL_STATE_NAME);

// Sélecteur pour obtenir la propriété 'showLoading' de l'état global
export const getLoading = createSelector(
  getGlobalState,
  (state) => {
    return state.showLoading;
  }
);
