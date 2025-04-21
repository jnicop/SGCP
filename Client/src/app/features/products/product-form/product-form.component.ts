import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductDto } from '../models/product.dto';
import { ProductService } from '../services/product.service';
import { ProductBuilderDto } from '../models/product-builder.model';

@Component({
  selector: 'app-product-form',
  standalone: false,
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent {
  productForm: FormGroup;
  categories = [
    { id: 1, name: 'Electronics' },
    { id: 2, name: 'Books' },
    { id: 3, name: 'Clothing' },
  ];
  constructor(private productService: ProductService,
    public dialogRef: MatDialogRef<ProductFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ProductDto | null,
    private fb: FormBuilder
  ) {
    this.productForm = this.fb.group({
      name: [data?.name || '', Validators.required],
      description: [data?.description || '', Validators.required],
      categoryId: [data?.categoryId || '', Validators.required],
    });
  
  }

  save(): void {
    if (this.productForm.invalid) return;
  

    const product: ProductBuilderDto = this.productForm.value;

    if (this.data?.id) {
      // Lógica de edición (se implementará después)
    } else {
   
this.productService.create(product).subscribe({
  next: () => {
    this.dialogRef.close(true); // Éxito
  },
  error: (err) => {
    console.error('Error creando producto:', err);
  }
});
    }
  }
  

  cancel(): void {
    this.dialogRef.close(null);
  }
  
}

