import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginResponseDto } from './dtos/login-response.dto';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Promise<void> {
    return this.http
      .post<LoginResponseDto>(`${this.apiUrl}/auth/login`, {
        username,
        password
      })
      .toPromise()
      .then(res => {
        if (!res) throw new Error('Respuesta vacía del servidor');
        localStorage.setItem('token', res.token);
        localStorage.setItem('username', res.username);
        localStorage.setItem('roles', JSON.stringify(res.roles));
      });
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('roles');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
