import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { LaborTypeDto } from 'app/models/labor-type.model';

@Injectable({
  providedIn: 'root'
})
export class LaborTypeService {
//   private apiUrl = 'https://localhost:5001/api/labortypes';
 private apiUrl = `${environment.apiUrl}/labortypes`;
  constructor(private http: HttpClient) {}

  getAll(): Observable<LaborTypeDto[]> {
    return this.http.get<LaborTypeDto[]>(this.apiUrl);
  }

  getById(id: number): Observable<LaborTypeDto> {
    return this.http.get<LaborTypeDto>(`${this.apiUrl}/${id}`);
  }
}
