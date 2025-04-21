import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CardWidgetComponent } from './components/card-widget/card-widget.component';
import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';
import { MaterialModule } from './material.module';
import { NoResultsComponent } from './components/no-results/no-results.component';
import { AppDialogComponent } from './components/app-dialog/app-dialog.component';
import { FilterComponent } from './components/filter/filter.component';
import { FormsModule } from '@angular/forms';
import { SummaryFooterComponent } from './components/summary-footer/summary-footer.component';
import { CustomCurrencyPipe } from './service/pipes/custom-currency.pipe';

@NgModule({
  declarations: [
    CardWidgetComponent,
    LoadingSpinnerComponent,
    NoResultsComponent,
    AppDialogComponent,
    SummaryFooterComponent,
    FilterComponent,
    CustomCurrencyPipe,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MaterialModule
  ],
  exports: [
    CardWidgetComponent,
    LoadingSpinnerComponent,
    MaterialModule,
    NoResultsComponent,
    SummaryFooterComponent,
    FilterComponent,
    CustomCurrencyPipe,
  ]
})
export class SharedModule { }
