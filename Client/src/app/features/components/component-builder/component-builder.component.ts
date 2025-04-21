import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, FormControl, AbstractControl } from '@angular/forms';
import { CategoryService } from 'app/services/category.service';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { DialogService } from 'shared/service/dialog.service';
import { SnackbarService } from 'shared/snackbar.service';
import { ComponentBuilderService } from '../services/component-builder.service';
import { CategoryDto } from 'app/models/category.model';
import { ComponentBuilderDto } from '../models/component-builder.dto';
import { AuxCatTypeService } from 'shared/service/auxCat.service';
import { AuxTypeDto } from 'core/dtos/AuxTypeDto';
import { UnitDto } from 'core/dtos/unit.dto';
import { CustomCurrencyPipe } from 'shared/service/pipes/custom-currency.pipe';
import { Observable, startWith, map } from 'rxjs';
import { createObjectFilterObservable, displayWithFn } from 'shared/utils/autocomplete.utils';

@Component({
  selector: 'app-component-builder',
  standalone:false,
  templateUrl: './component-builder.component.html',
  styleUrls: ['./component-builder.component.scss'],
  providers: [CustomCurrencyPipe]
})
export class ComponentBuilderComponent implements OnInit {
  builderForm!: FormGroup;
  weightForm!: FormGroup;
  presentationMode!: FormControl;
  presentationForm!: FormGroup;
  treatmentForm!: FormGroup;
  attributeForm!: FormGroup;
  processForm!: FormGroup;
  step = 1;
  defaultUnitId =1;
  isSaving = false;
  treatmentTypes: AuxTypeDto[] = [];
  processTypes: AuxTypeDto[] = [];
  scopeTypes: AuxTypeDto[] = [];


  componentId: number | null = null;
  isEditMode = false;
  isDuplicate  = false;

  categories: CategoryDto[] = [];
  categoryControl = new FormControl<CategoryDto | string>('');
  filteredCategories!: Observable<CategoryDto[]>;

  componentTypes: AuxTypeDto[] = [];
  componentTypesControl = new FormControl<AuxTypeDto | string>('');
  filteredComponentTypes!: Observable<AuxTypeDto[]>;

  unitTypes: UnitDto[] = [];
  unitTypesControl = new FormControl<AuxTypeDto | string>('');
  filteredUnitTypes!: Observable<AuxTypeDto[]>;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    private componentService: ComponentBuilderService,
    private auxCatService: AuxCatTypeService,
    private cdRef: ChangeDetectorRef,
    private dialogService: DialogService,
    private snackbar: SnackbarService,
    private router: Router,
    private customCurrencyPipe: CustomCurrencyPipe,
  ) {}

  ngOnInit(): void {
    this.presentationMode = this.fb.control('unidad'); 

    this.presentationForm = this.fb.group({
      description: [''],
      quantityPerBase: [1, [Validators.required, Validators.min(1)]],
      price: [0, [Validators.required, Validators.min(0)]],
      unitId: [this.defaultUnitId]
    });
    this.attributeForm = this.fb.group({
      attributeName: ['', Validators.required],
      attributeValue: ['', Validators.required]
    });
    this.treatmentForm = this.fb.group({
      treatmentTypeId: [null, Validators.required],
      extraCost: [0, [Validators.required, Validators.min(0)]]
    });
    this.processForm = this.fb.group({
      processTypeId: [null, Validators.required],
      scopeTypeId: [null, Validators.required],
      quantityApplied: [1, [Validators.required, Validators.min(0.01)]],
      costPerUnit: [0, [Validators.required, Validators.min(0)]],
      date: [new Date(), Validators.required],
      notes: ['']
    });
    
    this.weightForm = this.fb.group({
      weightGrams: [0, [Validators.required, Validators.min(0.1)]],
      pricePerGram: [0, [Validators.required, Validators.min(0.1)]],
      unitsPerTira: [0, [Validators.required, Validators.min(1)]]
    });
    this.presentationMode.valueChanges.subscribe(mode => {
      // this.presentations.clear();
      this.weightForm.reset();
    });

    const id = this.route.snapshot.paramMap.get('id');
     this.isDuplicate = this.route.snapshot.queryParamMap.get('duplicate') === 'true';

    if (id) {
      this.componentId = +id;
       this.isEditMode = !this.isDuplicate;
      this.loadComponent(this.componentId, this.isDuplicate);
    }

    forkJoin({
      categories: this.categoryService.getAll(),
      componentTypes: this.auxCatService.getComponentType(),
      treatmentTypes: this.auxCatService.getTreatmentType(),
      processTypes: this.auxCatService.getProccesType(),
      scopeTypes: this.auxCatService.getScopeType(),
      unitTypes: this.auxCatService.getUnitType()
    }).subscribe(({ categories, treatmentTypes, processTypes, scopeTypes,componentTypes,unitTypes }) => {
      this.categories = categories;
      this.componentTypes = componentTypes;
      this.treatmentTypes = treatmentTypes;
      this.processTypes = processTypes;
      this.scopeTypes = scopeTypes;
      this.unitTypes = unitTypes;
      if (!this.isEditMode) {
        this.categoryControl.setValue(''); 
        this.componentTypesControl.setValue(''); 
      }
   
         this.filteredCategories = createObjectFilterObservable<CategoryDto>(
           this.categoryControl,
           this.categories,
           cat => cat.name
         );
         this.filteredComponentTypes = createObjectFilterObservable<AuxTypeDto>(
          this.componentTypesControl,
          this.componentTypes,
          cat => cat.name
        );
        this.filteredUnitTypes = createObjectFilterObservable<AuxTypeDto>(
          this.unitTypesControl,
          this.unitTypes,
          cat => cat.name
        );
    });

    this.builderForm = this.fb.group({
      name: ['', Validators.required],
      code: [''],
      description: [''],
      categoryId: [null, Validators.required],
      componentTypeId: [null, Validators.required],
      unitId: [null],
      type: [''],
      enable: [true],
      unitCost: [0, Validators.required],
      presentations: this.fb.array([]),
      attributes: this.fb.array([]),
      treatments: this.fb.array([]),
      processes: this.fb.array([]),
      id
    });


  }

  
  displayCategory = displayWithFn<CategoryDto>(c => c.name);

  selectCategory(category: CategoryDto): void {
    this.builderForm.get('categoryId')?.setValue(category.id);
  }
  displayComponentType = displayWithFn<AuxTypeDto>(c => c.name);

  selectComponentType(componentType: AuxTypeDto): void {
    this.builderForm.get('componentTypeId')?.setValue(componentType.id);
  }
  displayUnitType = displayWithFn<AuxTypeDto>(c => c.name);

  selectUnitType(unitType: AuxTypeDto): void {
    this.builderForm.get('unitId')?.setValue(unitType.id);
  }

  addPresentationFromForm(): void {
    if (this.presentationForm.invalid) return;
  
    const { description, quantityPerBase, price, unitId } = this.presentationForm.value;
  
    this.presentations.push(this.fb.group({
      description: [description],
      quantityPerBase: [quantityPerBase],
      price: [price],
      unitId: [unitId],
      enable: [true]
    }));
  
    this.presentationForm.reset({ quantityPerBase: 1, price: 0, unitId: this.defaultUnitId });
    this.cdRef.detectChanges();
  }

  addPresentationFromWeight(): void {
    const { weightGrams, pricePerGram, unitsPerTira } = this.weightForm.value;
  
    const totalPrice = weightGrams * pricePerGram;
    const unitPrice = unitsPerTira > 0 ? totalPrice / unitsPerTira : 0;
  
    this.presentations.push(this.fb.group({
      description: [`Tira ${weightGrams}g / ${unitsPerTira}u`],
      quantityPerBase: [unitsPerTira],
      price: [totalPrice],
      unitId: [this.defaultUnitId],
      enable: [true]
    }));
  
    this.weightForm.reset();
  }

  addTreatmentFromForm(): void {
    if (this.treatmentForm.invalid) return;
  
    const { treatmentTypeId, extraCost } = this.treatmentForm.value;
  
    this.treatments.push(this.fb.group({
      treatmentTypeId: [treatmentTypeId],
      extraCost: [extraCost],
      enable: [true]
    }));
  
    this.treatmentForm.reset({ extraCost: 0 });
  }

  addAttributeFromForm(): void {
    if (this.attributeForm.invalid) return;
  
    const { attributeName, attributeValue } = this.attributeForm.value;
  
    this.attributes.push(this.fb.group({
      attributeName: [attributeName],
      attributeValue: [attributeValue],
      enable: [true]
    }));
  
    this.attributeForm.reset();
  }

  addProcessFromForm(): void {
    if (this.processForm.invalid) return;
  
    const value = this.processForm.value;
  
    this.processes.push(this.fb.group({
      processTypeId: [value.processTypeId],
      scopeTypeId: [value.scopeTypeId],
      quantityApplied: [value.quantityApplied],
      costPerUnit: [value.costPerUnit],
      date: [value.date],
      notes: [value.notes],
      enable: [true]
    }));
  
    this.processForm.reset({ quantityApplied: 1, costPerUnit: 0, date: new Date(), notes: '' });
  }
  

  getCategoryName(id: number): string {
    const cat = this.categories.find(c => c.id === id);
    return cat ? cat.name : 'Desconocida';
  }
  
  getTreatmentName(id: number): string {
    const t = this.treatmentTypes.find(x => x.id === id);
    return t ? t.name : 'Desconocido';
  }
  
  getProcessName(id: number): string {
    const p = this.processTypes.find(x => x.id === id);
    return p ? p.name : 'Desconocido';
  }
  
  getScopeName(id: number): string {
    const s = this.scopeTypes.find(x => x.id === id);
    return s ? s.name : 'Desconocido';
  }

  // submitComponent(): void {
  //   if (!this.builderForm.valid) {
  //     console.warn('Errores en el formulario:', this.builderForm.errors);
  //     console.warn('Controles con error:', this.builderForm);

  //     for (const controlName in this.builderForm.controls) {
  //       const control = this.builderForm.get(controlName);
  //       if (control?.invalid) {
  //         console.log(`❌ ${controlName} inválido:`, control.errors);
  //       }
  //     }


  //     this.markAllAsTouched(this.builderForm); // para mostrar los errores en pantalla
  //     return;
  //   }
  
  //   this.isSaving = true;
  
  //   const formValue = this.builderForm.value;
  
  //   const dto: ComponentBuilderDto = {
  //     name: formValue.name,
  //     code: formValue.code,
  //     description: formValue.description,
  //     categoryId: formValue.categoryId,
  //     componentTypeId: formValue.componentTypeId,
  //     unitId: formValue.unitId,
  //     type: formValue.type ?? '', // opcional, podés ajustar
  //     enable: formValue.enable ?? true,
  //     unitCost: this.unitFinalPrice, //formValue.unitCost,
  
  //     presentations: formValue.presentations.map((p: any) => ({
  //       code: p.code ?? '',
  //       description: p.description,
  //       measure: p.measure ?? '',
  //       quantityPerBase: p.quantityPerBase ?? 0,
  //       price: p.price,
  //       unitId: p.unitId,
  //       baseUnitCost: p.baseUnitCost ?? 0,
  //       weightGrams: p.weightGrams ?? 0,
  //       lengthMeters: p.lengthMeters ?? 0,
  //       enable: p.enable ?? true
  //     })),
  
  //     attributes: formValue.attributes.map((a: any) => ({
  //       attributeName: a.attributeName,
  //       attributeValue: a.attributeValue,
  //       enable: a.enable ?? true
  //     })),
  
  //     treatments: formValue.treatments.map((t: any) => ({
  //       treatmentTypeId: t.treatmentTypeId,
  //       extraCost: t.extraCost,
  //       enable: t.enable ?? true
  //     })),
  
  //     processes: formValue.processes.map((p: any) => ({
  //       processTypeId: p.processTypeId,
  //       scopeTypeId: p.scopeTypeId,
  //       quantityApplied: p.quantityApplied,
  //       costPerUnit: p.costPerUnit,
  //       date: p.date, // ya viene como Date
  //       notes: p.notes ?? ''
  //     }))
  //   };
  
  //   this.componentService.create(dto).subscribe({
  //     next: () => {
  //       this.snackbar.success('Componente creado correctamente.');
  //       this.builderForm.reset();
  //       this.presentations.clear();
  //       this.attributes.clear();
  //       this.treatments.clear();
  //       this.processes.clear();
  //       this.step = 1;
  //       this.router.navigate(['/components'], { state: { justSaved: true } });
  //     },
  //     error: () => {
  //       this.snackbar.error('Error al guardar el componente.');
  //     },
  //     complete: () => (this.isSaving = false)
  //   });
  // }
  submitComponent(): void {
    if (!this.builderForm.valid) {
      console.warn('Errores en el formulario:', this.builderForm.errors);
      this.markAllAsTouched(this.builderForm);
      return;
    }
  
    this.isSaving = true;
  
    const formValue = this.builderForm.value;
  
    const dto: ComponentBuilderDto = {
      name: formValue.name,
      code: formValue.code,
      description: formValue.description,
      categoryId: formValue.categoryId,
      componentTypeId: formValue.componentTypeId,
      unitId: formValue.unitId,
      type: formValue.type ?? '',
      enable: formValue.enable ?? true,
      unitCost: this.unitFinalPrice,
      // id: formValue.id ?? 0,
      id: this.isEditMode && !this.isDuplicate ? this.componentId : undefined,
      presentations: formValue.presentations.map((p: any) => ({
        code: p.code ?? '',
        description: p.description,
        measure: p.measure ?? '',
        quantityPerBase: p.quantityPerBase ?? 0,
        price: p.price,
        unitId: p.unitId,
        baseUnitCost: p.baseUnitCost ?? 0,
        weightGrams: p.weightGrams ?? 0,
        lengthMeters: p.lengthMeters ?? 0,
        enable: p.enable ?? true
      })),
  
      attributes: formValue.attributes.map((a: any) => ({
        attributeName: a.attributeName,
        attributeValue: a.attributeValue,
        enable: a.enable ?? true
      })),
  
      treatments: formValue.treatments.map((t: any) => ({
        treatmentTypeId: t.treatmentTypeId,
        extraCost: t.extraCost,
        enable: t.enable ?? true
      })),
  
      processes: formValue.processes.map((p: any) => ({
        processTypeId: p.processTypeId,
        scopeTypeId: p.scopeTypeId,
        quantityApplied: p.quantityApplied,
        costPerUnit: p.costPerUnit,
        date: p.date,
        notes: p.notes ?? '',
        enable: p.enable ?? true
      }))
    };
  
    // Si estás en modo duplicado, forzamos la creación aunque haya ID
    const request$ = this.isEditMode && !this.isDuplicate
    ? this.componentService.update(this.componentId!, dto)
    : this.componentService.create(dto);


    request$.subscribe({
      next: () => {
        this.snackbar.success(
          this.isEditMode && !this.isDuplicate ? 'Componente actualizado correctamente.' : 'Componente creado correctamente.'
        );
        this.router.navigate(['/components'], { state: { justSaved: true } });
      },
      error: () => {
        this.snackbar.error('Error al guardar el componente.');
      },
      complete: () => (this.isSaving = false)
    });
  }
  
  // loadComponent(id: number, isDuplicate: boolean): void {
  //   this.componentService.getById(id).subscribe({
  //     next: (component) => {
  //       this.builderForm.patchValue({
  //         name: isDuplicate ? `${component.name} (Copia)` : component.name,
  //         code: component.code,
  //         description: component.description,
  //         categoryId: component.categoryId,
  //         componentTypeId: component.componentTypeId,
  //         unitId: component.unitId,
  //         type: component.type,
  //         enable: component.enable,
  //         unitCost: component.unitCost,
  //         id: component.id
  //       });
  
  //       // Presentaciones
  //       component.presentations.forEach(p => {
  //         this.presentations.push(this.fb.group({
  //           description: [p.description],
  //           quantityPerBase: [p.quantityPerBase],
  //           price: [p.price],
  //           unitId: [p.unitId],
  //           enable: [p.enable ?? true]
  //         }));
  //       });
  
  //       // Atributos
  //       component.attributes.forEach(attr => {
  //         this.attributes.push(this.fb.group({
  //           attributeName: [attr.attributeName],
  //           attributeValue: [attr.attributeValue],
  //           enable: [attr.enable ?? true]
  //         }));
  //       });
  
  //       // Tratamientos
  //       component.treatments.forEach(treat => {
  //         this.treatments.push(this.fb.group({
  //           treatmentTypeId: [treat.treatmentTypeId],
  //           extraCost: [treat.extraCost],
  //           enable: [treat.enable ?? true]
  //         }));
  //       });
  
  //       // Procesos
  //       component.processes.forEach(proc => {
  //         this.processes.push(this.fb.group({
  //           processTypeId: [proc.processTypeId],
  //           scopeTypeId: [proc.scopeTypeId],
  //           quantityApplied: [proc.quantityApplied],
  //           costPerUnit: [proc.costPerUnit],
  //           date: [proc.date],
  //           notes: [proc.notes],
  //           enable: [proc.enable ?? true]
  //         }));
  //       });
  
  //       this.cdRef.detectChanges();
  //     },
  //     error: () => {
  //       this.dialogService.error('Error', 'No se pudo cargar el componente.');
  //       this.router.navigate(['/components']);
  //     }
  //   });
  // }
  private loadComponent(id: number, duplicate: boolean = false): void {
    this.componentService.getById(id).subscribe({
      next: (dto) => {
        if (duplicate) {
          dto.id=null,  
          dto.name += ' (copia)';
          dto.code = '';
        }
  
        this.builderForm.patchValue({
          name: dto.name,
          code: dto.code,
          description: dto.description,
          categoryId: dto.categoryId,
          componentTypeId: dto.componentTypeId,
          unitId: dto.unitId,
          type: dto.type ?? '',
          enable: dto.enable
        });

        const categoria = this.categories.find(c => c.id === dto.categoryId);
        if (categoria) {
          this.categoryControl.setValue(categoria); //  esto muestra el nombre en el combo
        }

        const componentType = this.categories.find(c => c.id === dto.componentTypeId);
        if (componentType) {
          this.categoryControl.setValue(componentType); //  esto muestra el nombre en el combo
        }

        const unitType = this.unitTypes.find(c => c.id === dto.unitId);
        if (unitType) {
          this.categoryControl.setValue(unitType); //  esto muestra el nombre en el combo
        }

        this.clearAllFormArrays();
  
        dto.presentations.forEach(p => this.presentations.push(this.fb.group({
          description: [p.description],
          quantityPerBase: [p.quantityPerBase],
          price: [p.price],
          unitId: [p.unitId],
          enable: [p.enable]
        })));
  
        dto.attributes.forEach(a => this.attributes.push(this.fb.group({
          attributeName: [a.attributeName],
          attributeValue: [a.attributeValue],
          enable: [a.enable]
        })));
  
        dto.treatments.forEach(t => this.treatments.push(this.fb.group({
          treatmentTypeId: [t.treatmentTypeId],
          extraCost: [t.extraCost],
          enable: [t.enable]
        })));
  
        dto.processes.forEach(proc => this.processes.push(this.fb.group({
          processTypeId: [proc.processTypeId],
          scopeTypeId: [proc.scopeTypeId],
          quantityApplied: [proc.quantityApplied],
          costPerUnit: [proc.costPerUnit],
          date: [new Date(proc.date)],
          notes: [proc.notes],
          enable: [true]
        })));
  
        this.cdRef.detectChanges();
      },
      error: () => {
        this.snackbar.error('Error al cargar el componente');
      }
    });
  }
  
  private clearAllFormArrays(): void {
    [this.presentations, this.attributes, this.treatments, this.processes].forEach(arr => {
      while (arr.length > 0) arr.removeAt(0);
    });
  }

  get presentations(): FormArray {
    return this.builderForm.get('presentations') as FormArray;
  }
  
  // addPresentation(): void {
  //   this.presentations.push(this.fb.group({
  //     description: [''],
  //     quantityPerBase: [1, [Validators.required, Validators.min(1)]], // cantidad total
  //     price: [0, [Validators.required, Validators.min(0)]],            // precio total del paquete
  //     unitId: [this.defaultUnitId],                                    // tu unidad por defecto
  //     enable: [true]
  //   }));
    
  // }
  
  
  get attributes(): FormArray {
    return this.builderForm.get('attributes') as FormArray;
  }
  
  // addAttribute(): void {
  //   this.attributes.push(this.fb.group({
  //     attributeName: ['', Validators.required],
  //     attributeValue: ['', Validators.required],
  //     enable: [true]
  //   }));
  // }
  


  get treatments(): FormArray {
    return this.builderForm.get('treatments') as FormArray;
  }
  
  // addTreatment(): void {
  //   this.treatments.push(this.fb.group({
  //     treatmentTypeId: [null, Validators.required],
  //     extraCost: [0, [Validators.required, Validators.min(0)]],
  //     enable: [true]
  //   }));
  // }

  
  get processes(): FormArray {
    return this.builderForm.get('processes') as FormArray;
  }
  
  // addProcess(): void {
  //   this.processes.push(this.fb.group({
  //     processTypeId: [null, Validators.required],
  //     scopeTypeId: [null, Validators.required],
  //     quantityApplied: [1, [Validators.required, Validators.min(0.01)]],
  //     costPerUnit: [0, [Validators.required, Validators.min(0)]],
  //     date: [new Date(), Validators.required],
  //     notes: ['']
  //   }));
  // }

  


  removeItem(formArray: FormArray, index: number): void {
    formArray.removeAt(index);
  }

  get totalBasePrice(): number {
    return this.presentations.controls.reduce(
      (acc, p) => acc + (+p.get('price')?.value || 0),
      0
    );
  }
  
  get totalTreatmentCost(): number {
    return this.treatments.controls.reduce(
      (acc, t) => acc + (+t.get('extraCost')?.value || 0),
      0
    );
  }
  
  get totalQuantity(): number {
    return this.presentations.controls.reduce(
      (acc, p) => acc + (+p.get('quantityPerBase')?.value || 0),
      0
    );
  }
  
  get unitPriceWithoutTreatment(): number {
    const qty = this.totalQuantity;
    return qty > 0 ? this.totalBasePrice / qty : 0;
  }
  
  get totalProcessCost(): number {
    return this.processes.controls.reduce(
      (acc, proc) =>
        acc + ((+proc.get('quantityApplied')?.value || 0) * (+proc.get('costPerUnit')?.value || 0)),
      0
    );
  }

  get unitFinalPrice(): number {
    const qty = this.totalQuantity;
    return qty > 0 ? (this.totalBasePrice + this.totalTreatmentCost + this.totalProcessCost) / qty : 0;
  }
  
  get totalFinalPrice(): number {
    return this.totalBasePrice + this.totalTreatmentCost + this.totalProcessCost;
  }
  
  get weightPreviewTotal(): number {
    const weight = this.weightForm.get('weightGrams')?.value || 0;
    const pricePerGram = this.weightForm.get('pricePerGram')?.value || 0;
    return weight * pricePerGram;
  }
  
  get weightPreviewUnitPrice(): number {
    const units = this.weightForm.get('unitsPerTira')?.value || 0;
    return units > 0 ? this.weightPreviewTotal / units : 0;
  }
  
  markAllAsTouched(group: FormGroup | FormArray): void {
    Object.values(group.controls).forEach(control => {
      if (control instanceof FormGroup || control instanceof FormArray) {
        this.markAllAsTouched(control);
      } else {
        control.markAsTouched();
        control.updateValueAndValidity();
      }
    });
  }


  getPresentationTotalFormatted(pres: AbstractControl): string {
    const group = pres as FormGroup;
    const price = +group.get('price')?.value || 0;
    const qty = +group.get('quantityPerBase')?.value || 0;
    const unit = this.unitTypes.find(u => u.id === group.get('unitId')?.value);
    const unitLabel = unit ? unit.symbol || unit.name : 'unidad';
  
    const unitPrice = qty > 0 ? price / qty : 0;
  
    return `${this.customCurrencyPipe.transform(price)} (${this.customCurrencyPipe.transform(unitPrice)} / ${unitLabel})`;
  }
  
  
  getUnitSymbol(unitId: number): string {
    const unit = this.unitTypes.find(u => u.id === unitId);
    return unit?.symbol || unit?.name || 'u';
  }
  
  formatCurrency(value: number): string {
    return new Intl.NumberFormat('es-AR', { style: 'currency', currency: 'ARS' }).format(value);
  }
  
  nextStep() { if (this.step < 6) this.step++; }
  prevStep() { if (this.step > 1) this.step--; }
}
