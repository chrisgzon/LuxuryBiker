import { UseCase } from '@base/use-case';
import { Observable } from 'rxjs';
import { DashboardModel } from '../models/dashboard.model';
import { DashboardRepository } from '../repositories/dashboard-repository';

export class GetDashboardUseCase implements UseCase<void, DashboardModel> {
  constructor(private dashboardRepository: DashboardRepository) {}

  execute(): Observable<DashboardModel> {
    return this.dashboardRepository.get();
  }
}
