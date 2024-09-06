export interface AuthTokenModel {
  authToken: string;
  expiresIn: Date;
  refreshToken: string;
  socketRoom: string;
}
