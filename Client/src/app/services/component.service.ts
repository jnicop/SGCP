import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import {ComponentDto} from 'features/components/models/component.dto';


@Injectable({
  providedIn: 'root'
})
export class ComponentService {
//   private apiUrl = 'https://localhost:5001/api/components';
 private apiUrl = `${environment.apiUrl}/components`;
  constructor(private http: HttpClient) {}

  getAll(): Observable<ComponentDto[]> {
    return this.http.get<ComponentDto[]>(this.apiUrl);
  }

  getById(id: number): Observable<ComponentDto> {
    return this.http.get<ComponentDto>(`${this.apiUrl}/${id}`);
  }

  create(component: ComponentDto): Observable<any> {
    return this.http.post(this.apiUrl, component);
  }

  update(id: number, component: ComponentDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, component);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
