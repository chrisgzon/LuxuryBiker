import { ThirdsImplementationRepository } from '@data/thirds/thirds-implementation.repository';
import { ThirdsRepository } from '@domain/thirds/repositories/thirds-repository';
import { ThirdCreateUseCase } from '@domain/thirds/useCases/create.useCase';

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

export const thirdsImplementationRepositoryProvider = { provide: ThirdsRepository, useClass: ThirdsImplementationRepository }