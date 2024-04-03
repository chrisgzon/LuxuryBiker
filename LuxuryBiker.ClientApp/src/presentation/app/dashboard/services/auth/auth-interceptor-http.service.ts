import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { first, switchMap } from 'rxjs';
import { AuthService } from './auth.service';

export const AuthInterceptorHttpService: HttpInterceptorFn = (req, next) => {

  const authService: AuthService = inject(AuthService);

  return authService.isLoggedIn$.pipe(
    first(),
    switchMap((isLoggedIn) => {
      if (isLoggedIn === false) {
        return next(req);
      }

      return authService.jwt$.pipe(
        first(Boolean),
        switchMap((jwt) => {
          const headers = req.headers.append(
            'Authorization',
            `Bearer ${jwt}`
          );
          return next(req.clone({ headers }));
        })
      );
    })
  );
}