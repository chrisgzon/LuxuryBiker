import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptorHttpService } from './services/auth/auth-interceptor-http.service';

import {
  authImplementationRespositoryProvider,
  getUserProfileUseCaseProvider,
  userLoginUseCaseProvider,
} from '@data/authentication';
import {
  getThirdsUseCaseProvider,
  getThirdByIdUseCaseProvider,
  updateThirdUseCaseProvider,
  thirdCreateUseCaseProvider,
  thirdsImplementationRepositoryProvider,
} from '@data/thirds';

import { TranslateModule } from '@ngx-translate/core';
import { provideTranslation } from './config/i18n/translate-loader.config';
import {
  getProductsUseCaseProvider,
  getProductByIdUseCaseProvider,
  updateProductUseCaseProvider,
  productCreateUseCaseProvider,
  productsImplementationRepositoryProvider,
} from '@data/products';
import {
  createPurchaseUseCaseProvider,
  getPurchasesUseCaseProvider,
  getPurchaseFormDataUseCaseProvider,
  changePurchaseStatusUseCaseProvider,
  purchasesImplementationRepositoryProvider,
} from '@data/purchases';
import {
  createSaleUseCaseProvider,
  getSalesUseCaseProvider,
  getSaleFormDataUseCaseProvider,
  changeSaleStatusUseCaseProvider,
  salesImplementationRepositoryProvider,
} from '@data/sales';
import {
  getDashboardUseCaseProvider,
  dashboardImplementationRepositoryProvider,
} from '@data/dashboard';
import { provideEnvironmentNgxCurrency } from 'ngx-currency';

const customCurrencyMaskConfig = {
  prefix: 'COP $ ',
  thousands: '.',
  allowNegative: false,
  precision: 0,
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([AuthInterceptorHttpService])),
    importProvidersFrom(TranslateModule.forRoot(provideTranslation())),
    provideEnvironmentNgxCurrency(customCurrencyMaskConfig),
    // auth service
    userLoginUseCaseProvider,
    getUserProfileUseCaseProvider,
    authImplementationRespositoryProvider,
    // thirds service
    thirdCreateUseCaseProvider,
    getThirdsUseCaseProvider,
    getThirdByIdUseCaseProvider,
    updateThirdUseCaseProvider,
    thirdsImplementationRepositoryProvider,
    // products service
    productCreateUseCaseProvider,
    getProductsUseCaseProvider,
    getProductByIdUseCaseProvider,
    updateProductUseCaseProvider,
    productsImplementationRepositoryProvider,
    // purchases service
    createPurchaseUseCaseProvider,
    getPurchasesUseCaseProvider,
    getPurchaseFormDataUseCaseProvider,
    changePurchaseStatusUseCaseProvider,
    purchasesImplementationRepositoryProvider,
    // sales service
    createSaleUseCaseProvider,
    getSalesUseCaseProvider,
    getSaleFormDataUseCaseProvider,
    changeSaleStatusUseCaseProvider,
    salesImplementationRepositoryProvider,
    // dashboard service
    getDashboardUseCaseProvider,
    dashboardImplementationRepositoryProvider,
  ],
};
