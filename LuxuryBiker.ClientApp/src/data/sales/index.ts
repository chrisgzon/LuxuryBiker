import { SalesRepository } from '@domain/sales/repositories/sales-repository';
import { CreateSaleUseCase } from '@domain/sales/useCases/create-sale.useCase';
import { GetSalesUseCase } from '@domain/sales/useCases/get-sales.useCase';
import { GetSaleFormDataUseCase } from '@domain/sales/useCases/get-sale-form-data.useCase';
import { ChangeSaleStatusUseCase } from '@domain/sales/useCases/change-sale-status.useCase';
import { SalesImplementationRepository } from './sales-implementation.repository';

const CreateSaleUseCaseFactory = (repo: SalesRepository) => new CreateSaleUseCase(repo);
export const createSaleUseCaseProvider = {
  provide: CreateSaleUseCase,
  useFactory: CreateSaleUseCaseFactory,
  deps: [SalesRepository],
};

const GetSalesUseCaseFactory = (repo: SalesRepository) => new GetSalesUseCase(repo);
export const getSalesUseCaseProvider = {
  provide: GetSalesUseCase,
  useFactory: GetSalesUseCaseFactory,
  deps: [SalesRepository],
};

const GetSaleFormDataUseCaseFactory = (repo: SalesRepository) => new GetSaleFormDataUseCase(repo);
export const getSaleFormDataUseCaseProvider = {
  provide: GetSaleFormDataUseCase,
  useFactory: GetSaleFormDataUseCaseFactory,
  deps: [SalesRepository],
};

const ChangeSaleStatusUseCaseFactory = (repo: SalesRepository) => new ChangeSaleStatusUseCase(repo);
export const changeSaleStatusUseCaseProvider = {
  provide: ChangeSaleStatusUseCase,
  useFactory: ChangeSaleStatusUseCaseFactory,
  deps: [SalesRepository],
};

export const salesImplementationRepositoryProvider = {
  provide: SalesRepository,
  useClass: SalesImplementationRepository,
};
