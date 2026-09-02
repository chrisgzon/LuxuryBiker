import { Observable } from 'rxjs';
import { DashboardModel } from '@domain/dashboard/models/dashboard.model';

export abstract class DashboardRepository {
  abstract get(): Observable<DashboardModel>;
}
