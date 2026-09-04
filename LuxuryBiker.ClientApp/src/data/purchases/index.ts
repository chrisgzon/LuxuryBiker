import { PurchasesRepository } from '@domain/purchases/repositories/purchases-repository';
import { CreatePurchaseUseCase } from '@domain/purchases/useCases/create-purchase.useCase';
import { GetPurchasesUseCase } from '@domain/purchases/useCases/get-purchases.useCase';
import { GetPurchaseFormDataUseCase } from '@domain/purchases/useCases/get-purchase-form-data.useCase';
import { ChangePurchaseStatusUseCase } from '@domain/purchases/useCases/change-purchase-status.useCase';
import { PurchasesImplementationRepository } from './purchases-implementation.repository';

const CreatePurchaseUseCaseFactory =
  (repo: PurchasesRepository) => new CreatePurchaseUseCase(repo);
export const createPurchaseUseCaseProvider = {
  provide: CreatePurchaseUseCase,
  useFactory: CreatePurchaseUseCaseFactory,
  deps: [PurchasesRepository],
};

const GetPurchasesUseCaseFactory =
  (repo: PurchasesRepository) => new GetPurchasesUseCase(repo);
export const getPurchasesUseCaseProvider = {
  provide: GetPurchasesUseCase,
  useFactory: GetPurchasesUseCaseFactory,
  deps: [PurchasesRepository],
};

const GetPurchaseFormDataUseCaseFactory =
  (repo: PurchasesRepository) => new GetPurchaseFormDataUseCase(repo);
export const getPurchaseFormDataUseCaseProvider = {
  provide: GetPurchaseFormDataUseCase,
  useFactory: GetPurchaseFormDataUseCaseFactory,
  deps: [PurchasesRepository],
};

const ChangePurchaseStatusUseCaseFactory =
  (repo: PurchasesRepository) => new ChangePurchaseStatusUseCase(repo);
export const changePurchaseStatusUseCaseProvider = {
  provide: ChangePurchaseStatusUseCase,
  useFactory: ChangePurchaseStatusUseCaseFactory,
  deps: [PurchasesRepository],
};

export const purchasesImplementationRepositoryProvider = {
  provide: PurchasesRepository,
  useClass: PurchasesImplementationRepository,
};
