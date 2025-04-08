import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AppDialogComponent } from '../components/app-dialog/app-dialog.component';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class DialogService {
  constructor(private dialog: MatDialog) {}

  confirm(title: string, message: string): Observable<boolean> {
    const dialogRef = this.dialog.open(AppDialogComponent, {
      width: '400px',
      data: { title, message, confirm: true }
    });
    return dialogRef.afterClosed();
  }

  alert(title: string, message: string): void {
    this.dialog.open(AppDialogComponent, {
      width: '400px',
      data: { title, message, confirm: false }
    });
  }

  error(title: string, message: string): void {
    this.dialog.open(AppDialogComponent, {
      width: '400px',
      data: { title, message, confirm: false }
    });
  }
}
