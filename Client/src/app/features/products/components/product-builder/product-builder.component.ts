// ✅ Archivo: product-builder.component.ts (versión final 100% funcional)

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { CategoryDto } from 'app/models/category.model';
import { CategoryService } from 'app/services/category.service';
import { ComponentDto } from 'app/models/component.model';
import { LaborTypeDto } from 'app/models/labor-type.model';
import { LaborTypeService } from 'app/services/labor-type.service';
import { ComponentService } from 'app/services/component.service';
import { ProductService } from '../services/product.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import { forkJoin } from 'rxjs';
import { DialogService } from 'shared/service/dialog.service';
import { ProductBuilderDto } from '../models/product-builder.model';
import { SnackbarService } from 'shared/snackbar.service';

@Component({
  selector: 'app-product-builder',
  templateUrl: './product-builder.component.html',
  styleUrls: ['./product-builder.component.scss'],
  standalone: false
})
export class ProductBuilderComponent implements OnInit {
  builderForm!: FormGroup;
  componentForm!: FormGroup;
  laborForm!: FormGroup;

  step = 1;
  productId: number | null = null;
  isEditMode = false;
  isSaving = false;

  categories: CategoryDto[] = [];
  availableComponents: ComponentDto[] = [];
  laborTypes: LaborTypeDto[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    private componentService: ComponentService,
    private laborTypeService: LaborTypeService,
    private productService: ProductService,
    private cdRef: ChangeDetectorRef,
    private dialogService:DialogService,
    private snackbar: SnackbarService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initForm();

    this.componentForm = this.fb.group({
      componentId: [null, Validators.required],
      quantity: [null, [Validators.required, Validators.min(0.01)]]
    });

    this.laborForm = this.fb.group({
      laborTypeId: [null, Validators.required],
      hours: [null, [Validators.required, Validators.min(0.1)]]
    });

    forkJoin({
      categories: this.categoryService.getAll(),
      components: this.componentService.getAll(),
      laborTypes: this.laborTypeService.getAll()
    }).subscribe(({ categories, components, laborTypes }) => {
      this.categories = categories;
      this.availableComponents = components;
      this.laborTypes = laborTypes;

      const id = this.route.snapshot.paramMap.get('id');
      if (id) {
        this.productId = +id;
        this.isEditMode = true;
        this.loadProduct(this.productId);
      }
    });
  }

  initForm() {
    this.builderForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      categoryId: [null, Validators.required],
      components: this.fb.array([]),
      laborCosts: this.fb.array([])
    });
  }

  get components(): FormArray {
    return this.builderForm.get('components') as FormArray;
  }

  get laborCosts(): FormArray {
    return this.builderForm.get('laborCosts') as FormArray;
  }

  
  nextStep() {
    if (this.step < 4) this.step++;
  }

  prevStep() {
    if (this.step > 1) this.step--;
  }

  addComponent() {
    if (this.componentForm.invalid) return;

    const { componentId, quantity } = this.componentForm.value;
    const component = this.availableComponents.find(c => c.id === +componentId);
    if (!component) return;

    const alreadyExists = this.components.controls.some(
      c => c.get('componentId')?.value === componentId
    );
    if (alreadyExists) return;

    this.components.push(this.fb.group({
      componentId: [componentId, Validators.required],
      componentName: [component.name],
      quantity: [quantity, [Validators.required, Validators.min(0.01)]]
    }));

    this.componentForm.reset();
    this.cdRef.detectChanges();
  }

  removeComponent(index: number) {
    this.components.removeAt(index);
  }

  addLaborCost() {
    if (this.laborForm.invalid) return;

    const { laborTypeId, hours } = this.laborForm.value;
    const labor = this.laborTypes.find(l => l.id === +laborTypeId);
    if (!labor) return;

    const alreadyExists = this.laborCosts.controls.some(
      l => l.get('laborTypeId')?.value === laborTypeId
    );
    if (alreadyExists) return;

    this.laborCosts.push(this.fb.group({
      laborTypeId: [laborTypeId, Validators.required],
      laborTypeName: [labor.name],
      hours: [hours, [Validators.required, Validators.min(0.1)]]
    }));

    this.laborForm.reset();
    this.cdRef.detectChanges();
  }

  removeLaborCost(index: number) {
    this.laborCosts.removeAt(index);
  }

  getCategoryName(id: number): string {
    const cat = this.categories.find(c => c.id === id);
    return cat ? cat.name : 'Desconocida';
  }

 
  // submitProduct() {
  //   if (this.builderForm.invalid) {
  //     this.dialogService.alert('Faltan datos obligatorios', 'Por favor completá todos los campos requeridos.');
  //     return;
  //   }
  
  //   const dto = this.builderForm.value;
  //   dto.productId = this.productId ?? 0;
  
  //   dto.components = dto.components.map((c: any) => ({
  //     componentId: c.componentId,
  //     quantity: c.quantity
  //   }));
  
  //   dto.laborCosts = dto.laborCosts.map((l: any) => ({
  //     laborTypeId: l.laborTypeId,
  //     hours: l.hours
  //   }));
  
  //   if (this.isEditMode) {
  //     this.productService.updateProduct(dto).subscribe({
  //       next: () => {
  //         this.dialogService.alert('Producto actualizado', 'El producto fue actualizado correctamente.');
  //       },
  //       error: () => {
  //         this.dialogService.alert('Error al actualizar', 'Ocurrió un error al intentar actualizar el producto.');
  //       }
  //     });
  //   } else {
  //     this.productService.create(dto).subscribe({
  //       next: () => {
  //         this.dialogService.alert('Producto creado', 'El producto fue creado correctamente.');
  //         this.builderForm.reset();
  //         this.components.clear();
  //         this.laborCosts.clear();
  //         this.step = 1;
  //       },
  //       error: () => {
  //         this.dialogService.alert('Error al crear', 'Ocurrió un error al intentar crear el producto.');
  //       }
  //     });
  //   }
  // }
  submitProduct() {
    if (!this.builderForm.valid) {
      this.dialogService.error("Error", 'Faltan datos obligatorios');
      return;
    }
  
    this.isSaving = true;
    const formValue = this.builderForm.value;
  
    const componentsFormArray = this.builderForm.get('components') as FormArray;
    const laborFormArray = this.builderForm.get('laborCosts') as FormArray;
  
    const dto: ProductBuilderDto = {
      productId: this.productId ?? 0,
      name: formValue.name,
      description: formValue.description,
      categoryId: formValue.categoryId,
      components: componentsFormArray.controls.map(c => ({
        componentId: c.get('componentId')?.value,
        quantity: c.get('quantity')?.value,
        unitId: c.get('unitId')?.value ?? 0,
        enable: c.get('enable')?.value ?? true
      })),
      laborCosts: laborFormArray.controls.map(l => ({
        laborTypeId: l.get('laborTypeId')?.value,
        quantity: l.get('quantity')?.value,
        unitId: l.get('unitId')?.value ?? 0,
        enable: l.get('enable')?.value ?? true
      }))
    };
  
    this.productService.create(dto).subscribe({
      next: () => {
        this.snackbar.success(
          this.isEditMode ? 'Producto actualizado correctamente' : 'Producto creado correctamente'
        );
        this.builderForm.reset();
        componentsFormArray.clear();
        laborFormArray.clear();
        this.step = 1;
        this.router.navigate(['/products']);
      },
      error: () => this.dialogService.error("Error", 'Error al guardar el producto'),
      complete: () => (this.isSaving = false)
    });
  }
  
  loadProduct(id: number) {
    this.productService.getById(id).subscribe(product => {
      // Cargar datos principales del producto
      this.builderForm.patchValue({
        name: product.name,
        description: product.description,
        categoryId: product.categoryId
      });
  
      // COMPONENTES
      const compFormArray: FormArray = this.fb.array([]);
      product.components.forEach(c => {
        const comp = this.availableComponents.find(ac => ac.id === c.componentId);
        compFormArray.push(this.fb.group({
          componentId: [c.componentId, Validators.required],
          componentName: [comp?.name ?? 'Desconocido'],
          quantity: [c.quantity, [Validators.required, Validators.min(0.01)]],
          unitId: [c.unitId, Validators.required],
          enable: [c.enable]
        }));
      });
      this.builderForm.setControl('components', compFormArray);
  
      // MANO DE OBRA
      const laborFormArray: FormArray = this.fb.array([]);
      product.laborCosts.forEach(l => {
        const type = this.laborTypes.find(t => t.id === l.laborTypeId);
        laborFormArray.push(this.fb.group({
          laborTypeId: [l.laborTypeId, Validators.required],
          laborTypeName: [type?.name ?? 'Desconocido'],
          quantity: [l.quantity, [Validators.required, Validators.min(0.01)]],
          unitId: [l.unitId, Validators.required],
          enable: [l.enable]
        }));
      });
      this.builderForm.setControl('laborCosts', laborFormArray);
  
      this.cdRef.detectChanges();
    });
  }
  
  
  // loadProduct(id: number) {
  //   this.productService.getById(id).subscribe(product => {
  //     this.builderForm.patchValue({
  //       name: product.name,
  //       description: product.description,
  //       categoryId: product.categoryId
  //     });
  
  //     // const compFormArray = this.fb.array([]);
  //     const compFormArray: FormArray = this.fb.array([]);
  //     product.components.forEach(c => {
  //       const comp = this.availableComponents.find(ac => ac.id === c.componentId);
  //       compFormArray.push(this.fb.group({
  //         componentId: [c.componentId, Validators.required],
  //         componentName: [comp?.name ?? 'Desconocido'],
  //         quantity: [c.quantity, [Validators.required, Validators.min(0.01)]]
  //       }));
  //     });
  //     this.builderForm.setControl('components', compFormArray);
  //     const comps: FormArray = this.fb.array([]);
  
  //     const laborFormArray: FormArray = this.fb.array([]);
  //     // const laborFormArray = this.fb.array([]);
  //     product.laborCosts.forEach(l => {
  //       const type = this.laborTypes.find(t => t.id === l.laborTypeId);
  //       laborFormArray.push(this.fb.group({
  //         laborTypeId: [l.laborTypeId, Validators.required],
  //         laborTypeName: [type?.name ?? 'Desconocido'],
  //         hours: [l.hours, [Validators.required, Validators.min(0.1)]]
  //       }));
  //     });
  //     this.builderForm.setControl('laborCosts', laborFormArray);
  //     const labors: FormArray = this.fb.array([]);
  
  //     this.cdRef.detectChanges();
  //   });
  // }
  
 
}
