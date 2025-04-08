import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ProductDto } from '../models/product.dto';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { PaginationQueryDto } from 'core/dtos/PaginationQueryDto';
import { PagedResult } from 'core/dtos/PagedResult';
import { ProductCreateDto } from '../models/ProductCreateDto';
import { ProductBuilderDto } from '../models/product-builder.model';
import { ProductFilterDto } from '../models/product-filter.dto';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private apiUrl = `${environment.apiUrl}/products`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ProductBuilderDto[]> {
    return this.http.get<ProductBuilderDto[]>(this.apiUrl);
  }
  // getPagedProducts(query: PaginationQueryDto): Observable<PagedResult<ProductDto>> {
  //   return this.http.post<PagedResult<ProductDto>>(`${this.apiUrl}/paged`, query);
  // }

  // getById(id: number): Observable<ProductDto> {
  //   return this.http.get<ProductDto>(`${this.apiUrl}/${id}`);
  // }

  create(product: ProductBuilderDto): Observable<ProductDto> {
    return this.http.post<ProductDto>(`${this.apiUrl}/builder`, product);
  }

  update(id: number, product: ProductDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, product);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }


  getBuilder(id: number): Observable<ProductBuilderDto> {
    return this.http.get<ProductBuilderDto>(`${this.apiUrl}/builder/${id}`);
  } 

  getById(id: number): Observable<ProductBuilderDto> {
    return this.http.get<ProductBuilderDto>(`${this.apiUrl}/builder/${id}`);
  }
  
  updateProduct( dto: ProductBuilderDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/builder`, dto);
  }

  setProductEnable(id: number, enable: boolean): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/enable?enable=${enable}`, {});
  }

  getPagedProducts(filter: ProductFilterDto): Observable<PagedResult<ProductDto>> {
    let params = new HttpParams()
      .set('pageIndex', filter.pageIndex)
      .set('pageSize', filter.pageSize)
      .set('search', filter.search || '');
  
    if (filter.enable !== null && filter.enable !== undefined) {
      params = params.set('enable', filter.enable.toString());
    }
  
    return this.http.post<PagedResult<ProductDto>>(`${this.apiUrl}/paged`, { params });
  }
  
}