import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoginCredentialsModel } from '@models/auth/login-credentials.model';
import { UserLoggedModel } from '@models/auth/user-logged.model';
import { UserModel } from '@models/user.model';
import { BehaviorSubject, Observable, ignoreElements, map, tap } from 'rxjs';
import { UserLoginUseCase } from '@useCases/auth/user-login.useCase';

const USER_LOCAL_STORAGE_KEY = 'userData';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user = new BehaviorSubject<UserLoggedModel | null>(null);
  user$ = this.user.asObservable();
  isLoggedIn$: Observable<boolean> = this.user$.pipe(map(Boolean));

  constructor(private userLoginUseCase: UserLoginUseCase, private router: Router) {
    this.loadUserFromLocalStorage();
  }

  login(credentials: LoginCredentialsModel): Observable<never> {
    return this.userLoginUseCase.execute(credentials)
    .pipe(
      tap((user: UserLoggedModel) => this.saveTokenToLocalStore(user.token)),
      tap((user: UserLoggedModel) => this.pushNewUser(user.token)),
      tap(() => this.redirectToDashboard()),
      ignoreElements()
    );
  }

  logout(): void {
    this.removeUserFromLocalStorage();
    this.user.next(null);
    this.router.navigateByUrl('/login');
  }

  private redirectToDashboard(): void {
    this.router.navigateByUrl('/home');
  }

  private pushNewUser(token: string) {
    this.user.next(this.decodeToken(token));
  }

  private decodeToken(userToken: string): UserLoggedModel {
    const userInfo = JSON.parse(window.atob(userToken)) as UserModel;

    return { ...userInfo, token: userToken };
  }
  private loadUserFromLocalStorage(): void {
    const userFromLocal = localStorage.getItem(USER_LOCAL_STORAGE_KEY);

    userFromLocal && this.pushNewUser(userFromLocal);
  }
  private saveTokenToLocalStore(userToken: string): void {
    localStorage.setItem(USER_LOCAL_STORAGE_KEY, userToken);
  }

  private removeUserFromLocalStorage(): void {
    localStorage.removeItem(USER_LOCAL_STORAGE_KEY);
  }
}
