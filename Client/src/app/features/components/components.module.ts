import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { SharedModule } from "shared/shared.module";
import { RouterModule, Routes } from "@angular/router";
import { NgModule } from "@angular/core";

import { ComponentBuilderComponent } from "./component-builder/component-builder.component";
import { ComponentListComponent } from './component-list/component-list.component';

const routes: Routes = [
     { path: '', component: ComponentListComponent },
    { path: 'builder', component: ComponentBuilderComponent },
     { path: 'builder/:id', component: ComponentBuilderComponent } 
  ];
  
@NgModule({
    declarations: [ ComponentBuilderComponent, ComponentListComponent],
    imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RouterModule.forChild(routes)
]
  })
  export class ComponentsModule {}
  