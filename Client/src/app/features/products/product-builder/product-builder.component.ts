// ✅ Archivo: product-builder.component.ts (versión final 100% funcional)

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, FormControl } from '@angular/forms';
import { CategoryDto } from 'app/models/category.model';
import { CategoryService } from 'app/services/category.service';
import { ComponentDto } from 'features/components/models/component.dto';
import { LaborTypeDto } from 'app/models/labor-type.model';
import { LaborTypeService } from 'app/services/labor-type.service';
import { ComponentService } from 'app/services/component.service';
import { ProductService } from '../services/product.service';
import { CurrencyConfigService } from 'shared/service/currency-config.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import { forkJoin, Observable } from 'rxjs';
import { DialogService } from 'shared/service/dialog.service';
import { ProductBuilderDto } from '../models/product-builder.model';
import { SnackbarService } from 'shared/snackbar.service';
import { AuxCatTypeService } from 'shared/service/auxCat.service';
import { UnitDto } from 'core/dtos/unit.dto';
import { CustomCurrencyPipe } from 'shared/service/pipes/custom-currency.pipe';
import { createObjectFilterObservable, displayWithFn } from 'shared/utils/autocomplete.utils';


@Component({
  selector: 'app-product-builder',
  templateUrl: './product-builder.component.html',
  styleUrls: ['./product-builder.component.scss'],
  standalone: false,
  providers: [CustomCurrencyPipe]
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
  categoryControl = new FormControl<CategoryDto | string>('');
  filteredCategories!: Observable<CategoryDto[]>;

  availableComponents: ComponentDto[] = [];
  laborTypes: LaborTypeDto[] = [];
 unitTypes: UnitDto[] = [];
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    private componentService: ComponentService,
    private laborTypeService: LaborTypeService,
    private productService: ProductService,
    private customCurrencyPipe: CustomCurrencyPipe,
    private auxCatService: AuxCatTypeService,
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
      categories: this.categoryService.getAllByType(1),
      components: this.componentService.getAll(),
      laborTypes: this.laborTypeService.getAll(),
      unitTypes: this.auxCatService.getUnitType(),
    }).subscribe(({ categories, components, laborTypes,unitTypes }) => {
      this.categories = categories;
      this.availableComponents = components;
      this.laborTypes = laborTypes;
      this.unitTypes = unitTypes;
      const id = this.route.snapshot.paramMap.get('id');

      if (id) {
        this.productId = +id;
        this.isEditMode = true;
        this.loadProduct(this.productId);
      }

      if (!this.isEditMode) {
        this.categoryControl.setValue(''); // fuerza la emisión y muestra todas las categorías
      }

      this.filteredCategories = createObjectFilterObservable<CategoryDto>(
        this.categoryControl,
        this.categories,
        cat => cat.name
      );
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

  displayCategory = displayWithFn<CategoryDto>(c => c.name);

  selectCategory(category: CategoryDto): void {
    this.builderForm.get('categoryId')?.setValue(category.id);
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
  
    const unit = this.unitTypes.find(u => u.id === component.unitId);
    const unitLabel = unit ? unit.symbol || unit.name : 'unidad';

    this.components.push(this.fb.group({
      componentId: [componentId, Validators.required],
      componentName: [component.name],
      quantity: [quantity, [Validators.required, Validators.min(0.01)]],
      unitId: [component.unitId ?? null],
      unit:unitLabel,
      enable: [true]
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

    const unit = this.unitTypes.find(u => u.id === labor.unitId);
    const unitLabel = unit ? unit.symbol || unit.name : 'h';
    
    this.laborCosts.push(this.fb.group({
      laborTypeId: [laborTypeId, Validators.required],
      laborTypeName: [labor.name],
      quantity: [hours, [Validators.required, Validators.min(0.1)]], // 👈 usamos `quantity` en lugar de `hours`
      unitId: [labor.unitId ?? null],
      unit: unitLabel,
      enable: [true]
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

  getComponentUnitAndCost(componentId: number): string {
    const component = this.availableComponents.find(c => c.id === componentId);
    if (!component) return '';
  
    const unit = this.unitTypes.find(u => u.id === component.unitId);
    const unitLabel = unit ? unit.symbol || unit.name : 'unidad';
    return `${this.customCurrencyPipe.transform(component.unitCost)} / ${unitLabel}`;
  }
  
  getComponentTotalLineCost(componentId: number, quantity: number): string {
    const component = this.availableComponents.find(c => c.id === componentId);
    if (!component) return '';
  
    const total = component.unitCost * quantity;
    return `${this.customCurrencyPipe.transform(total)}`;
  }
 
  get summaryComponents(): any[] {
    return this.components.controls.map(ctrl => ({
      componentName: ctrl.get('componentName')?.value,
      quantity: ctrl.get('quantity')?.value,
      unit: ctrl.get('unit')?.value,
      componentId: ctrl.get('componentId')?.value,
    }));
  }
  
  get summaryLabor(): any[] {
    return this.laborCosts.controls.map(ctrl => ({
      laborTypeName: ctrl.get('laborTypeName')?.value,
      quantity: ctrl.get('quantity')?.value,
      laborTypeId: ctrl.get('laborTypeId')?.value
    }));
  }
  
  getLaborLineCostDetail(laborTypeId: number, quantity: number): string {
    const labor = this.laborTypes.find(l => l.id === laborTypeId);
    if (!labor) return '';
    const unitCost = labor.hourlyCost;
    const total = unitCost * quantity;
  
    return `${this.customCurrencyPipe.transform(total)} (${this.customCurrencyPipe.transform(unitCost)}/h)`;

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
      const categoria = this.categories.find(c => c.id === product.categoryId);
      if (categoria) {
        this.categoryControl.setValue(categoria); //  esto muestra el nombre en el combo
      }
        
      // COMPONENTES
      const compFormArray: FormArray = this.fb.array([]);
      product.components.forEach(c => {
        const comp = this.availableComponents.find(ac => ac.id === c.componentId);
        const unit = this.unitTypes.find(u => u.id === c.unitId);
        const unitLabel = unit ? unit.symbol || unit.name : 'unidad';
        
        compFormArray.push(this.fb.group({
          componentId: [c.componentId, Validators.required],
          componentName: [comp?.name ?? 'Desconocido'],
          quantity: [c.quantity, [Validators.required, Validators.min(0.01)]],
          unit: unitLabel,
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
  
  formatCurrencyARS(value: number): string {
    return new Intl.NumberFormat(
    ).format(value);
  }
  
  get componentTotalCost(): number {
    return this.components.controls.reduce((acc, ctrl) => {
      const componentId = ctrl.get('componentId')?.value;
      const quantity = +ctrl.get('quantity')?.value || 0;
      const component = this.availableComponents.find(c => c.id === componentId);
      const unitCost = component?.unitCost ?? 0;
      return acc + (quantity * unitCost);
    }, 0);
  }
  
  get laborTotalCost(): number {
    return this.laborCosts.controls.reduce((acc, ctrl) => {
      const laborTypeId = ctrl.get('laborTypeId')?.value;
      const quantity = +ctrl.get('hours')?.value || +ctrl.get('quantity')?.value || 0;
      const labor = this.laborTypes.find(l => l.id === laborTypeId);
      const hourlyCost = labor?.hourlyCost ?? 0;
      return acc + (quantity * hourlyCost);
    }, 0);
  }
  
  get productTotalCost(): number {
    return this.componentTotalCost + this.laborTotalCost;
  }

  getUnitCost(componentId: number): string {
    const component = this.availableComponents.find(c => c.id === componentId);
    if (!component) return '';
    return `${this.customCurrencyPipe.transform( component.unitCost)}`;
  }
  
  getHourlyCost(laborTypeId: number): string {
    const labor = this.laborTypes.find(l => l.id === laborTypeId);
    if (!labor) return '';
    return `${this.customCurrencyPipe.transform( labor.hourlyCost)}`;
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
