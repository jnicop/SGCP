import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuxTypeDto } from "core/dtos/AuxTypeDto";
import { UnitDto } from "core/dtos/unit.dto";
import { environment } from "environments/environment";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class AuxCatTypeService {
  private baseUrl = `${environment.apiUrl}/catalogs`;
  private unitsUrl = `${environment.apiUrl}/units`;

  constructor(private http: HttpClient) {}

  getProccesType(): Observable<AuxTypeDto[]> {
    return this.http.get<AuxTypeDto[]>(`${this.baseUrl}/process-types`);
  }
  getScopeType(): Observable<AuxTypeDto[]> {
    return this.http.get<AuxTypeDto[]>(`${this.baseUrl}/scope-types`);
  }
  getTreatmentType(): Observable<AuxTypeDto[]> {
    return this.http.get<AuxTypeDto[]>(`${this.baseUrl}/treatment-types`);
  }
  getComponentType(): Observable<AuxTypeDto[]> {
    return this.http.get<AuxTypeDto[]>(`${this.baseUrl}/component-types`);
  }
  getUnitType(): Observable<UnitDto[]> {
    return this.http.get<UnitDto[]>(`${this.unitsUrl}/unit-types`);
  }
}