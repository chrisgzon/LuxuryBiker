import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptorHttpService } from './services/auth/auth-interceptor-http.service';

import { authImplementationRespositoryProvider, getUserProfileUseCaseProvider, userLoginUseCaseProvider } from '@data/authentication';
import { thirdCreateUseCaseProvider, thirdsImplementationRepositoryProvider } from '@data/thirds';

import { TranslateModule } from '@ngx-translate/core';
import { provideTranslation } from './config/i18n/translate-loader.config';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([AuthInterceptorHttpService])),
    userLoginUseCaseProvider,
    getUserProfileUseCaseProvider,
    authImplementationRespositoryProvider,
    thirdCreateUseCaseProvider,
    thirdsImplementationRepositoryProvider,
    importProvidersFrom(TranslateModule.forRoot(provideTranslation())),
  ]
};
