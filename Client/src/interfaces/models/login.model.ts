export interface LoginModel {
  authToken: string;
  expiresIn: Date;
  refreshToken: string;
  socketRoom: string;
}
