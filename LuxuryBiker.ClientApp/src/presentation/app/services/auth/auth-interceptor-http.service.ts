import { inject } from '@angular/core';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, EMPTY, first, switchMap } from 'rxjs';
import { AuthService } from './auth.service';

export const AuthInterceptorHttpService: HttpInterceptorFn = (req, next) => {

  const authService: AuthService = inject(AuthService);

  return authService.isLoggedIn$.pipe(
    first(),
    switchMap((isLoggedIn) => {
      if (isLoggedIn === false) {
        return next(req);
      }

      return authService.jwt$
      .pipe(
        first(Boolean),
        switchMap((jwt) => {
          const headers = req.headers.append(
            'Authorization',
            `Bearer ${jwt}`
          );
          
          return next(req.clone({ headers }))
          .pipe(
            catchError((error: HttpErrorResponse) => {
              if (error.status === 401) {
                authService.logout();
                return EMPTY;
              }
              throw error;
            })
          )
        })
      );
    })
  );
}