export interface AuthTokenArgs {
  user?: string | null;
  password?: string | null;
  confirmationCode?: string | null;
  refreshToken?: string | null;
  registerToken?: string | null;
}
