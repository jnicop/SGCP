import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss'],
  standalone:false
})
export class FilterComponent {
  filterText: string = '';

  @Output() filterChanged = new EventEmitter<string>();

  onInputChange() {
    this.filterChanged.emit(this.filterText.trim());
  }

  clear() {
    this.filterText = '';
    this.filterChanged.emit('');
  }
}
