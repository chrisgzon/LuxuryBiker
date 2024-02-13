import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoginCredentialsModel } from '@domain/authentication/models/login-credentials.model';
import { UserLoggedModel } from '@domain/authentication/models/user-logged.model';
import { BehaviorSubject, Observable, catchError, finalize, ignoreElements, isEmpty, map, tap } from 'rxjs';
import { UserLoginUseCase } from '@domain/authentication/useCases/user-login.useCase';
import { GetUserProfileUseCase } from '@domain/authentication/useCases/get-user-profile.useCase';
import { HttpErrorResponse } from '@angular/common/http';

const USER_LOCAL_STORAGE_KEY = 'userData';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user = new BehaviorSubject<UserLoggedModel | null>(null);
  private jwt = new BehaviorSubject<string | null>(null);
  jwt$ = this.jwt.asObservable();
  user$ = this.user.asObservable();
  isLoggedIn$: Observable<boolean> = this.jwt$.pipe(map(Boolean));

  constructor(
    private userLoginUseCase: UserLoginUseCase,
    private getUserProfileUseCase: GetUserProfileUseCase,
    private router: Router)
  {
    this.loadJwtFromLocalStorage();
  }

  login(credentials: LoginCredentialsModel): Observable<never> {
    return this.userLoginUseCase.execute(credentials)
    .pipe(
      tap((user: UserLoggedModel) => this.saveTokenToLocalStore(user.token)),
      tap((user: UserLoggedModel) => this.pushNewJwt(user.token)),
      tap((user: UserLoggedModel) => this.pushNewUser(user)),
      tap(() => this.redirectToDashboard()),
      ignoreElements()
    );
  }

  logout(): void {
    this.removeUserFromLocalStorage();
    this.user.next(null);
    this.router.navigateByUrl('/login');
  }

  getProfileCurrentUserIfNotIsSet(): void {
    const userFromLocal = localStorage.getItem(USER_LOCAL_STORAGE_KEY);

    userFromLocal && this.user
                        .pipe(
                          tap(user => !user && this.loadUserFromLocalStorage())
                        ).subscribe()
  }

  private loadUserFromLocalStorage(): void {
    this.getUserProfileUseCase.execute()
    .pipe(
      tap((userProfile: UserLoggedModel) => this.pushNewUser(userProfile)),
      ignoreElements(),
      catchError((error: HttpErrorResponse) => {
        throw error;
      })
    ).subscribe();
  }

  private loadJwtFromLocalStorage(): void {
    const jwtFromLocal = localStorage.getItem(USER_LOCAL_STORAGE_KEY);

    jwtFromLocal && this.pushNewJwt(jwtFromLocal);
  }

  private redirectToDashboard(): void {
    this.router.navigateByUrl('/home');
  }

  private pushNewJwt(jwt: string) {
    this.jwt.next(jwt);
  }

  private pushNewUser(userProfile: UserLoggedModel) {
    this.user.next(userProfile);
  }


  private saveTokenToLocalStore(userToken: string): void {
    localStorage.setItem(USER_LOCAL_STORAGE_KEY, userToken);
  }

  private removeUserFromLocalStorage(): void {
    localStorage.removeItem(USER_LOCAL_STORAGE_KEY);
  }
}
