import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './app-dialog.component.html',
  styleUrls: ['./app-dialog.component.scss'],
  standalone:false
})
export class AppDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<AppDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { title: string; message: string; confirm: boolean }
  ) {}
}
