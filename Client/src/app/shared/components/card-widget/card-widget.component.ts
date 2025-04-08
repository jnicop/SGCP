import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-card-widget',
  templateUrl: './card-widget.component.html',
  styleUrls: ['./card-widget.component.scss'],
  standalone: false
  // imports: []
})
export class CardWidgetComponent {
  @Input() title!: string;
  @Input() value!: string | number;
  @Input() icon: string = 'info';
  @Input() color: 'primary' | 'accent' | 'warn' | 'success' | 'info' = 'primary';
}