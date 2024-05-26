import { createAction, createActionGroup, props } from "@ngrx/store";
import { CurrentUser } from "../_models/currentUser";
import { ForgotPassword } from "../_models/forgotPassword";
import { Login } from "../_models/login";
import { Register } from "../_models/register";
import { ResetPassword } from "../_models/resetPassword";

/**
 * Constantes définissant les préfixes des actions liées à l'authentification et aux e-mails.
 */
export const AUTH = '[AUTH]'; // Préfixe pour les actions d'authentification
export const MAIL = '[MAIL]'; // Préfixe pour les actions liées aux e-mails


// Actions liées à l'authentification
// Actions pour le processus de connexion
export const login_start = 'login start'; // Début du processus de connexion
export const login_success = 'login Success'; // Succès de la connexion
export const login_fail = 'login fail'; // Échec de la connexion

// Actions pour le processus d'inscription
export const signup_start = 'sign up start'; // Début du processus d'inscription
export const signup_success = 'sign up success'; // Succès de l'inscription

// Actions pour le processus de réinitialisation du mot de passe
export const reset_password_start = 'reset password start'; // Début du processus de réinitialisation du mot de passe
export const reset_password_success = 'reset password success'; // Succès de la réinitialisation du mot de passe

// Actions pour l'autologin et la déconnexion
export const auto_login = AUTH + 'auto login'; // Action d'autologin
export const logout_action = AUTH + 'logout'; // Action de déconnexion

// Action Update current user
export const update_user_action = AUTH + 'update current user'

// Actions liées à l'envoi d'e-mails
// Actions pour le processus d'envoi d'e-mail
export const send_mail_start = 'sendMail start'; // Début du processus d'envoi d'e-mail
export const send_mail_success = 'sendMail success'; // Succès de l'envoi d'e-mail


/**
 * Actions regroupées pour le processus de connexion.
 */
export const LoginAction = createActionGroup({
  source: AUTH,
  events: {
    login_start: props<{ login: Login }>(),
    login_success: props<{ currentUser: CurrentUser; redirect: boolean }>(),
    login_fail: props < {error: string} >()
  }
});

/**
 * Actions regroupées pour le processus d'inscription.
 */
export const SignupAction = createActionGroup({
  source: AUTH,
  events: {
    signup_start: props<{ register: Register }>(),
    signup_success: props<{ register: Register, redirect: boolean }>()
  }
});

/**
 * Action pour l'autologin de l'utilisateur.
 */
export const autoLogin = createAction(auto_login);

/**
 * Action pour la déconnexion de l'utilisateur.
 */
export const logout = createAction(logout_action);

/**
 * Actions regroupées pour le processus de réinitialisation du mot de passe.
 */
export const ResetPasswordAction = createActionGroup({
  source: AUTH,
  events: {
    reset_password_start: props<{ resetPassword: ResetPassword }>(),
    reset_password_success: props<{ redirect: boolean }>()
  }
})

/**
 * Action pour mettre à jour le current user
 */
export const updateCurrentUser = createAction(update_user_action, props<{ currentUser: CurrentUser }>());


/**
 * Actions regroupées pour le processus d'envoi d'e-mails.
 */
export const SendMailAction = createActionGroup({
  source: MAIL,
  events: {
    send_mail_start: props<{ forgotPassword: ForgotPassword }>(),
    send_mail_success: props<{ sendMail: boolean }>()    
  }
})



