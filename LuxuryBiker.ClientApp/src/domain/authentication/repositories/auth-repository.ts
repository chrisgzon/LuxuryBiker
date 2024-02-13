import { LoginCredentialsModel } from "../models/login-credentials.model";
import { Observable } from "rxjs";
import { UserLoggedModel } from "../models/user-logged.model";

export abstract class AuthRepository {
  abstract login(params: LoginCredentialsModel): Observable<UserLoggedModel>;
  abstract getProfileCurrentUser(): Observable<UserLoggedModel>;
}