import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { SnackbarService } from '../shared/snackbar.service'; // ajustá si tu path es diferente

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private router: Router,
    private snackbarService: SnackbarService
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();

    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 || error.status === 403) {
          // Guardar la URL actual para volver después del login
          const returnUrl = window.location.pathname + window.location.search;
          localStorage.setItem('returnUrl', returnUrl);
        
          this.authService.logout();
          this.snackbarService.error('Tu sesión ha expirado. Iniciá sesión nuevamente.');
          this.router.navigate(['/login']);
        }

        return throwError(() => error);
      })
    );
  }
}
