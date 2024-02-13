import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptorHttpService } from '@services/auth/auth-interceptor-http.service';
import { authImplementationRespositoryProvider, getUserProfileUseCaseProvider, userLoginUseCaseProvider } from '@data/authentication';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([AuthInterceptorHttpService])),
    userLoginUseCaseProvider,
    getUserProfileUseCaseProvider,
    authImplementationRespositoryProvider
  ]
};
