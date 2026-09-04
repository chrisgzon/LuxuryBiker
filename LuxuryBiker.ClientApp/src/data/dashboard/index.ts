import { DashboardRepository } from '@domain/dashboard/repositories/dashboard-repository';
import { GetDashboardUseCase } from '@domain/dashboard/useCases/get-dashboard.useCase';
import { DashboardImplementationRepository } from './dashboard-implementation.repository';

const GetDashboardUseCaseFactory = (repo: DashboardRepository) => new GetDashboardUseCase(repo);
export const getDashboardUseCaseProvider = {
  provide: GetDashboardUseCase,
  useFactory: GetDashboardUseCaseFactory,
  deps: [DashboardRepository],
};

export const dashboardImplementationRepositoryProvider = {
  provide: DashboardRepository,
  useClass: DashboardImplementationRepository,
};
