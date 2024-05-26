// Interface définissant la structure de l'état global
export interface GlobalState {
  showLoading: boolean; // Propriété indiquant l'état du chargement
}

// État initial de l'application, conforme à l'interface GlobalState
export const initialState: GlobalState = {
  showLoading: false, // Initialisation de la propriété showLoading à false
};
