import { CommonModule } from "@angular/common";
import { ProductListComponent } from "../products/components/product-list/product-list.component";
import { FormsModule } from "@angular/forms";
import { SharedModule } from "shared/shared.module";
import { RouterModule, Routes } from "@angular/router";
import { NgModule } from "@angular/core";
import { ProductFilterComponent } from './components/product-filter/product-filter.component';
import { ProductFormComponent } from './components/product-form/product-form.component';
import { ProductBuilderComponent } from './components/product-builder/product-builder.component';

const routes: Routes = [
    { path: '', component: ProductListComponent },
    { path: 'builder', component: ProductBuilderComponent }, // 👈 Ruta al builder
    { path: 'builder/:id', component: ProductBuilderComponent } 
  ];
  
@NgModule({
    declarations: [ProductListComponent, ProductFilterComponent, ProductFormComponent, ProductBuilderComponent],
    imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RouterModule.forChild(routes)
]
  })
  export class ProductsModule {}
  