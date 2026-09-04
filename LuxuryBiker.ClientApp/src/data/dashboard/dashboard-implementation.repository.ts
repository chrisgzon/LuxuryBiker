import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { DashboardRepository } from '@domain/dashboard/repositories/dashboard-repository';
import { DashboardModel } from '@domain/dashboard/models/dashboard.model';

@Injectable({
  providedIn: 'root',
})
export class DashboardImplementationRepository extends DashboardRepository {
  constructor(private http: HttpClient) {
    super();
  }

  get(): Observable<DashboardModel> {
    return this.http.get<DashboardModel>(`${environment.apiUrl}/Dashboard/GetData`);
  }
}
