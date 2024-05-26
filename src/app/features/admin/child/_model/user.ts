import { Role } from "./role";

/**
 * La classe `User` représente un utilisateur dans l'application.
 * Elle stocke des informations sur un utilisateur, telles que son identifiant unique, son numéro d'identification,
 * son nom d'utilisateur, son prénom, son nom de famille, son adresse e-mail, et son rôle.
 */
export class User {
  /**
   * Identifiant unique de l'utilisateur.
   * @type {string}
   */
  id!: string;

  /**
   * Numéro d'identification de l'utilisateur.
   * @type {number}
   */
  numberId!: number;

  /**
   * Nom d'utilisateur de l'utilisateur.
   * @type {string}
   */
  userName!: string;

  /**
   * Prénom de l'utilisateur.
   * @type {string}
   */
  firstname!: string;

  /**
   * Nom de famille de l'utilisateur.
   * @type {string}
   */
  lastname!: string;

  /**
   * Adresse e-mail de l'utilisateur.
   * @type {string}
   */
  email!: string;

  /**
   * Rôle de l'utilisateur.
   * @type {string}
   */
  role!: Role;
}
