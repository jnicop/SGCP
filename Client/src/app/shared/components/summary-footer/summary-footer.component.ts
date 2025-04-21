import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-summary-footer',
  templateUrl: './summary-footer.component.html',
  styleUrls: ['./summary-footer.component.scss'],
  standalone: false
})
export class SummaryFooterComponent {
  @Input() totalBase: number = 0;
  @Input() totalTreatment: number = 0;
  @Input() totalFinal: number = 0;
  @Input() unitBase: number = 0;
  @Input() unitFinal: number = 0;
  @Input() insumos: number = 0;
  @Input() manoObra: number = 0;

  @Input() showBase: boolean = true;
  @Input() showTreatment: boolean = true;
  @Input() showFinal: boolean = true;
  @Input() showUnit: boolean = true;
  @Input() showUnitFinal: boolean = true;
  @Input() showInsumos: boolean = true;
  @Input() showManoObra:boolean = true;
}