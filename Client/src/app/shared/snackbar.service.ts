import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class SnackbarService {
  constructor(private snackBar: MatSnackBar) {}

  success(message: string): void {
    this.snackBar.open(`✅ ${message}`, 'Cerrar', {
      duration: 3000,
      panelClass: ['snackbar-success'],
      horizontalPosition: 'end',
      verticalPosition: 'bottom'
    });
  }

  error(message: string): void {
    this.snackBar.open(`❌ ${message}`, 'Cerrar', {
      duration: 3000,
      panelClass: ['snackbar-error'],
      horizontalPosition: 'end',
      verticalPosition: 'bottom'
    });
  }

  info(message: string): void {
    this.snackBar.open(`ℹ️ ${message}`, 'Cerrar', {
      duration: 3000,
      panelClass: ['snackbar-info'],
      horizontalPosition: 'end',
      verticalPosition: 'bottom'
    });
  }
}
