import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable, map } from 'rxjs';
import { ThirdImplementationRepositoryMapper } from '@data/thirds/third-implementation-repository.mapper';
import { ThirdEntity } from '@data/thirds/third.entity';
import { GetThirdsParams, ThirdsRepository } from '@domain/thirds/repositories/thirds-repository';
import { ThirdModel } from '@domain/thirds/models/third.model';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';

@Injectable({
  providedIn: 'root',
})
export class ThirdsImplementationRepository extends ThirdsRepository {
  thirdMapper = new ThirdImplementationRepositoryMapper();

  constructor(private http: HttpClient) {
    super();
  }

  create(params: ThirdModel): Observable<number> {
    const thirdEntity = this.thirdMapper.mapTo(params);
    return this.http.post<number>(
      `${environment.apiUrl}/Thirds/Create`,
      thirdEntity
    );
  }

  getById(id: number): Observable<ThirdModel> {
    return this.http
      .get<ThirdEntity>(`${environment.apiUrl}/Thirds/GetById`, {
        params: new HttpParams().set('id', String(id)),
      })
      .pipe(map((entity) => this.thirdMapper.mapFrom(entity)));
  }

  update(params: ThirdModel): Observable<void> {
    const entity = this.thirdMapper.mapTo(params);
    return this.http.post<void>(`${environment.apiUrl}/Thirds/Update`, entity);
  }

  getAll(params: GetThirdsParams): Observable<PaginatedResult<ThirdModel>> {
    let httpParams = new HttpParams()
      .set('pageNumber', String(params.pageNumber ?? 1))
      .set('pageSize', String(params.pageSize ?? 20));

    if (params.typeId != null) {
      httpParams = httpParams.set('typeId', String(params.typeId));
    }

    return this.http
      .get<PaginatedResult<ThirdEntity>>(`${environment.apiUrl}/Thirds/GetAll`, {
        params: httpParams,
      })
      .pipe(
        map((response) => ({
          ...response,
          items: response.items.map((item) => this.thirdMapper.mapFrom(item)),
        }))
      );
  }
}
