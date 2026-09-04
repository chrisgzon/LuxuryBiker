import { ThirdsImplementationRepository } from '@data/thirds/thirds-implementation.repository';
import { ThirdsRepository } from '@domain/thirds/repositories/thirds-repository';
import { ThirdCreateUseCase } from '@domain/thirds/useCases/create.useCase';
import { GetThirdsUseCase } from '@domain/thirds/useCases/get-thirds.useCase';
import { GetThirdByIdUseCase } from '@domain/thirds/useCases/get-third-by-id.useCase';
import { UpdateThirdUseCase } from '@domain/thirds/useCases/update-third.useCase';

export * from '@data/thirds/thirds-implementation.repository';
export * from '@data/thirds/third-implementation-repository.mapper';
export * from '@data/thirds/third.entity';

const ThirdCreateUseCaseFactory =
(thirdRepo: ThirdsRepository) => new ThirdCreateUseCase(thirdRepo);
export const thirdCreateUseCaseProvider = {
    provide: ThirdCreateUseCase,
    useFactory: ThirdCreateUseCaseFactory,
    deps: [ThirdsRepository],
};

const GetThirdsUseCaseFactory =
(thirdRepo: ThirdsRepository) => new GetThirdsUseCase(thirdRepo);
export const getThirdsUseCaseProvider = {
    provide: GetThirdsUseCase,
    useFactory: GetThirdsUseCaseFactory,
    deps: [ThirdsRepository],
};

const GetThirdByIdUseCaseFactory =
(thirdRepo: ThirdsRepository) => new GetThirdByIdUseCase(thirdRepo);
export const getThirdByIdUseCaseProvider = {
    provide: GetThirdByIdUseCase,
    useFactory: GetThirdByIdUseCaseFactory,
    deps: [ThirdsRepository],
};

const UpdateThirdUseCaseFactory =
(thirdRepo: ThirdsRepository) => new UpdateThirdUseCase(thirdRepo);
export const updateThirdUseCaseProvider = {
    provide: UpdateThirdUseCase,
    useFactory: UpdateThirdUseCaseFactory,
    deps: [ThirdsRepository],
};

export const thirdsImplementationRepositoryProvider = { provide: ThirdsRepository, useClass: ThirdsImplementationRepository }
