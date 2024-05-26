/**
 * Classe statique définissant les URL de l'API backend.
 */
export class URLBACKEND {
  // URL de base de l'API backend
  public static readonly API_BASE_URL = "https://localhost:7165/api/";

  //Authentification
  private static readonly AUTH: string = "Auth/";
  public static readonly AUTH_LOGIN: string = URLBACKEND.API_BASE_URL + URLBACKEND.AUTH + "login";
  public static readonly AUTH_REGISTER: string = URLBACKEND.API_BASE_URL + URLBACKEND.AUTH + "register";
  public static readonly AUTH_FORGOT_PASSWORD: string = URLBACKEND.API_BASE_URL + URLBACKEND.AUTH + "ForgotPassword";
  public static readonly AUTH_RESET_PASSWORD: string = URLBACKEND.API_BASE_URL + URLBACKEND.AUTH + "ResetPassword";
  public static readonly AUTH_CHANGE_PASSWORD: string = URLBACKEND.API_BASE_URL + URLBACKEND.AUTH + "UpdatePassword";

  //Token
  public static readonly TOKEN_REFRESH: string = URLBACKEND.API_BASE_URL + "Token/Refresh"

  //User
  public static readonly USER_UPDATE_ROLE_USER: string = URLBACKEND.API_BASE_URL + "User/UpdateRoleUser/"

  //Product
  public static readonly PRODUCT_GET_BY_CATEGORIES: string = URLBACKEND.API_BASE_URL + "Product/ProductsByCategoryIds"

  //Stock
  public static readonly STOCK_ADDMANY: string = URLBACKEND.API_BASE_URL + "Stock/AddMany"
} 
