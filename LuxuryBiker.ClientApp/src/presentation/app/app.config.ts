import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { UserRepository } from '@abstract/repositories/user-repository';
import { UserLoginUseCase } from '@useCases/auth/user-login.useCase';
import { UserImplementationRepository } from '@implementation/repositories/user-implementation.repository';
import { provideHttpClient } from '@angular/common/http';

const userLoginUseCaseFactory = 
(userRepo: UserRepository) => new UserLoginUseCase(userRepo);
const userLoginUseCaseProvider = {
    provide: UserLoginUseCase,
    useFactory: userLoginUseCaseFactory,
    deps: [UserRepository],
};


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    userLoginUseCaseProvider,
    { provide: UserRepository, useClass: UserImplementationRepository }
  ]
};
