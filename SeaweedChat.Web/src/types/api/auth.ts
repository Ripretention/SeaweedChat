export interface AuthResult {
  id: string;
  sessionToken: string;
}
export interface AuthParams {
  email: string;
  password: string;
}
export interface SignUpParams {
  email: string;
  password: string;
  username: string;
}
