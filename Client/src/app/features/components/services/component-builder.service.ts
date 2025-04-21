import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ComponentBuilderDto } from '../models/component-builder.dto';
import { ComponentDto } from '../models/component.dto'
import { environment } from 'environments/environment';
import { PagedResult } from 'core/dtos/PagedResult';
import { ComponentFilterDto } from '../models/ComponentFilterDto';

@Injectable({ providedIn: 'root' })
export class ComponentBuilderService {
  private baseUrl = `${environment.apiUrl}/components`;

  constructor(private http: HttpClient) {}

  create(dto: ComponentBuilderDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/builder`, dto);
  }

  update(id: number, dto: ComponentBuilderDto): Observable<any> {
    return this.http.put(`${this.baseUrl}/builder/${id}`, dto);
  }

  getById(id: number): Observable<ComponentBuilderDto> {
    return this.http.get<ComponentBuilderDto>(`${this.baseUrl}/${id}`);
  }

  getAll(): Observable<ComponentDto[]> {
    return this.http.get<ComponentDto[]>(`${this.baseUrl}/paged`);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  setEnable(id: number, enable: boolean): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${id}/enable`, { enable });
  }
  getPagedComponents(filter: ComponentFilterDto): Observable<PagedResult<ComponentDto>> {
    let params = new HttpParams()
      .set('pageIndex', filter.pageIndex)
      .set('pageSize', filter.pageSize)
      .set('search', filter.search || '');
  
    if (filter.enable !== null && filter.enable !== undefined) {
      params = params.set('enable', filter.enable.toString());
    }
    // params = params.set('categoryId', '').set('componentTypeId', '');
    return this.http.get<PagedResult<ComponentDto>>(`${this.baseUrl}/paged`, { params });
  }


  

}

