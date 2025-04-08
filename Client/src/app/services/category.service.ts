import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { CategoryDto } from '../models/category.model';


@Injectable({
  providedIn: 'root'
})
export class CategoryService {
//   private apiUrl = 'https://localhost:5001/api/categories';
 private apiUrl = `${environment.apiUrl}/categories`;
  constructor(private http: HttpClient) {}

  getAll(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(this.apiUrl);
  }

  getById(id: number): Observable<CategoryDto> {
    return this.http.get<CategoryDto>(`${this.apiUrl}/${id}`);
  }

  create(category: CategoryDto): Observable<any> {
    return this.http.post(this.apiUrl, category);
  }

  update(id: number, category: CategoryDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, category);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
