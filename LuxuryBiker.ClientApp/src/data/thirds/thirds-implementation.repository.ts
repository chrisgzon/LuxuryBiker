import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@enviroments/enviroment';
import { Observable, map } from 'rxjs';
import { ThirdImplementationRepositoryMapper } from '@data/thirds/third-implementation-repository.mapper';
import { ThirdsRepository } from '@domain/thirds/repositories/thirds-repository';
import { ThirdModel } from '@domain/thirds/models/third.model';

@Injectable({
  providedIn: 'root',
})
export class ThirdsImplementationRepository extends ThirdsRepository {
  thirdMapper = new ThirdImplementationRepositoryMapper();

  constructor(private http: HttpClient) {
    super();
  }

  create(params: ThirdModel): Observable<number> {
    const thirdEntity = this.thirdMapper
      .mapTo(params);
    return this.http.post<number>(
      `${environment.apiUrl}/Thirds/Create`,
      thirdEntity
    );
  }
}
