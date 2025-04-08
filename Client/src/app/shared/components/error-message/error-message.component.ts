import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-error-message',
  standalone: false,
  template: `<div *ngIf="message" class="error-message">{{ message }}</div>`,
  styles: [`.error-message { color: #f44336; font-size: 13px; margin-top: 4px; }`]
})
export class ErrorMessageComponent {
  @Input() message: string | null = null;
}
