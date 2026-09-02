import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DashboardModel } from '@domain/dashboard/models/dashboard.model';
import { GetDashboardUseCase } from '@domain/dashboard/useCases/get-dashboard.useCase';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  constructor(private getDashboardUseCase: GetDashboardUseCase) {}

  get(): Observable<DashboardModel> {
    return this.getDashboardUseCase.execute();
  }
}
