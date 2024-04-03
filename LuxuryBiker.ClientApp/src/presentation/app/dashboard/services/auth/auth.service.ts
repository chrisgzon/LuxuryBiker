import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoginCredentialsModel } from '@domain/authentication/models/login-credentials.model';
import { UserLoggedModel } from '@domain/authentication/models/user-logged.model';
import { BehaviorSubject, Observable, catchError, ignoreElements, map, tap } from 'rxjs';
import { UserLoginUseCase } from '@domain/authentication/useCases/user-login.useCase';
import { GetUserProfileUseCase } from '@domain/authentication/useCases/get-user-profile.useCase';
import { HttpErrorResponse } from '@angular/common/http';

const USER_LOCAL_STORAGE_KEY = 'userData';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user = new BehaviorSubject<UserLoggedModel | null>(null);
  user$ = this.user.asObservable();
  private jwt = new BehaviorSubject<string | null>(null);
  jwt$ = this.jwt.asObservable();
  isLoggedIn$: Observable<boolean> = this.jwt$.pipe(map(Boolean));

  constructor(
    private userLoginUseCase: UserLoginUseCase,
    private getUserProfileUseCase: GetUserProfileUseCase,
    private router: Router)
  {
    this.loadUserFromLocalStorage();
  }

  login(credentials: LoginCredentialsModel): Observable<never> {
    return this.userLoginUseCase.execute(credentials)
    .pipe(
      tap((token: string) => this.saveTokenToLocalStore(token)),
      tap((token: string) => this.pushNewJWT(token)),
      tap(() => this.redirectToDashboard()),
      ignoreElements()
    );
  }

  logout(): void {
    this.removeUserFromLocalStorage();
    this.router.navigateByUrl('/login');
  }
  
  getProfileCurrentUser(): Observable<never> {
    return this.getUserProfileUseCase.execute()
    .pipe(
      tap((userProfile: UserLoggedModel) => this.pushNewUser(userProfile)),
      ignoreElements(),
      catchError((error: HttpErrorResponse) => {
        this.removeUserFromLocalStorage();
        throw error;
      })
    );
  }

  private getToken(): string|null {
    return localStorage.getItem(USER_LOCAL_STORAGE_KEY);
  }
    
  private loadUserFromLocalStorage(): void {
    const jwtFromLocal = this.getToken();
    
    jwtFromLocal
    && this.pushNewJWT(jwtFromLocal);
  }
  
  private redirectToDashboard(): void {
    this.router.navigateByUrl('/home');
  }

  private saveTokenToLocalStore(userToken: string): void {
    localStorage.setItem(USER_LOCAL_STORAGE_KEY, userToken);
  }

  private pushNewUser(userProfile: UserLoggedModel|null) {
    this.user.next(userProfile);
  }

  private pushNewJWT(jwt: string|null) {
    this.jwt.next(jwt);
  }

  private removeUserFromLocalStorage(): void {
    localStorage.removeItem(USER_LOCAL_STORAGE_KEY);
    this.pushNewUser(null);
    this.pushNewJWT(null);
  }
}
