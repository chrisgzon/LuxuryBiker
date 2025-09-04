import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptorHttpService } from './services/auth/auth-interceptor-http.service';

import { authImplementationRespositoryProvider, getUserProfileUseCaseProvider, userLoginUseCaseProvider } from '@data/authentication';
import { thirdCreateUseCaseProvider, thirdsImplementationRepositoryProvider } from '@data/thirds';

import { TranslateModule } from '@ngx-translate/core';
import { provideTranslation } from './config/i18n/translate-loader.config';
import { productCreateUseCaseProvider, productsImplementationRepositoryProvider } from '@data/products';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([AuthInterceptorHttpService])),
    importProvidersFrom(TranslateModule.forRoot(provideTranslation())),
    // auth service
    userLoginUseCaseProvider,
    getUserProfileUseCaseProvider,
    authImplementationRespositoryProvider,
    // thirds service
    thirdCreateUseCaseProvider,
    thirdsImplementationRepositoryProvider,
    // products service
    productCreateUseCaseProvider,
    productsImplementationRepositoryProvider
  ]
};
