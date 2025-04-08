import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-no-results',
  templateUrl: './no-results.component.html',
  styleUrls: ['./no-results.component.scss'],
  standalone: false
})
export class NoResultsComponent {
  @Input() message = 'No se encontraron resultados.';
  @Input() showButton = false;
  @Input() buttonText = 'Recargar';

  @Output() buttonClick = new EventEmitter<void>();

  onClick() {
    this.buttonClick.emit();
  }
}
