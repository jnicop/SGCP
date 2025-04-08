import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CardWidgetComponent } from './components/card-widget/card-widget.component';
import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';
import { MaterialModule } from './material.module';
import { NoResultsComponent } from './components/no-results/no-results.component';
import { AppDialogComponent } from './components/app-dialog/app-dialog.component';


@NgModule({
  declarations: [
    CardWidgetComponent,
    LoadingSpinnerComponent,
    NoResultsComponent,
    AppDialogComponent
  ],
  imports: [
    CommonModule,
    MaterialModule
  ],
  exports: [
    CardWidgetComponent,
    LoadingSpinnerComponent,
    MaterialModule,
    NoResultsComponent
  ]
})
export class SharedModule { }
