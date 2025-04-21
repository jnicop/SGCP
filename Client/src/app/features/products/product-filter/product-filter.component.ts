import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-product-filter',
  templateUrl: './product-filter.component.html',
  styleUrls: ['./product-filter.component.scss'],
  standalone:false
})
export class ProductFilterComponent {
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
