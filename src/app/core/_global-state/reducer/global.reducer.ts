import { createReducer, on } from "@ngrx/store";
import { initialState } from "../state/global.state";
import { setLoadingSpinner } from "../action/global.action";

// Définition du réducteur global pour gérer les actions
const _globalReducer = createReducer(
  initialState,
  on(setLoadingSpinner, (state, action) => {
    return {
      ...state,
      showLoading: action.status,
    };
  }) 
);

// Fonction de réduction globale exportée
export function GlobalReducer(state: any, action: any) {
  return _globalReducer(state, action);
}
