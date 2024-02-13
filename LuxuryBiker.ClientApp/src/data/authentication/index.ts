import { AuthRepository } from '@domain/authentication/repositories/auth-repository';
import { GetUserProfileUseCase } from '@domain/authentication/useCases/get-user-profile.useCase';
import { UserLoginUseCase } from '@domain/authentication/useCases/user-login.useCase';
import { AuthImplementationRepository } from './auth-implementation.repository';

export * from './auth-implementation.repository';
export * from './user-auth-implementation-repository.mapper';
export * from './user-logged.entity';

const userLoginUseCaseFactory = 
(userRepo: AuthRepository) => new UserLoginUseCase(userRepo);
export const userLoginUseCaseProvider = {
    provide: UserLoginUseCase,
    useFactory: userLoginUseCaseFactory,
    deps: [AuthRepository],
};

const getUserProfileUseCaseFactory = 
(userRepo: AuthRepository) => new GetUserProfileUseCase(userRepo);
export const getUserProfileUseCaseProvider = {
    provide: GetUserProfileUseCase,
    useFactory: getUserProfileUseCaseFactory,
    deps: [AuthRepository],
};

export const authImplementationRespositoryProvider = { provide: AuthRepository, useClass: AuthImplementationRepository }