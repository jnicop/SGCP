import { Component, OnInit, ViewChild } from '@angular/core';
import { ProductService } from '../services/product.service';
import { ProductDto } from '../models/product.dto';
import { MatDialog } from '@angular/material/dialog';
// import { ProductFormComponent } from '../product-form/product-form.component';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { PaginationQueryDto } from 'core/dtos/PaginationQueryDto';
import { ProductBuilderComponent } from '../product-builder/product-builder.component';
import { SnackbarService } from 'shared/snackbar.service';
// import { ConfirmDialogComponent } from 'app/shared/components/confirm-dialog/confirm-dialog.component';
import { DialogService } from 'shared/service/dialog.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
  standalone:false
})
export class ProductListComponent implements OnInit {
  products: ProductDto[] = [];
  filteredProducts: ProductDto[] = [];
  paginatedProducts: ProductDto[] = [];
  totalItems=0;
  pageIndex = 0;
  pageSize = 10;
  isLoading = true;
  

  dataSource = new MatTableDataSource<ProductDto>();
  searchText = '';
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = ['name', 'finalPrice', 'category', 'actions'];
  constructor(private productService: ProductService,private dialog: MatDialog,   private snackbar: SnackbarService, private dialogService:DialogService,
    private router: Router) {}

  ngOnInit(): void {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras.state as { justSaved?: boolean };
  
    if (state?.justSaved) {
      setTimeout(() => {
        this.snackbar.success('Producto guardado exitosamente');
      }, 100);
    }

    this.loadPagedProducts();
  }

  // loadProducts(): void {
  //   this.productService.getAll().subscribe({
  //     next: (data) => {
  //       this.products = data;
  //       this.filteredProducts = data;
  //       this.updatePaginatedProducts();
  //     },
  //     error: (err) => {
  //       console.error('Error loading products:', err);
  //     }
  //   });
  // }
  loadPagedProducts(): void {
    this.isLoading = true;
    const query: PaginationQueryDto = {
      pageIndex: this.pageIndex,
      pageSize: this.pageSize,
      search: this.searchText,
      enable: true
    };
  
    this.productService.getPagedProducts(query).subscribe({
      next: result => {
        this.filteredProducts = result.items;
        this.totalItems = result.totalItems;
        this.updatePaginatedProducts();
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Error loading products:', err);
      }
    });
  }
  paginate(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadPagedProducts();
  }
  
  filterProducts(): void {
    const text = this.searchText.toLowerCase();
    this.filteredProducts = this.products.filter(p =>
      p.name.toLowerCase().includes(text)
    );
    this.pageIndex = 0; // Volver a la primera página
    this.updatePaginatedProducts();
  }
  

  applyFilter(term: string) {
  this.filteredProducts = this.products.filter(p =>
    p.name.toLowerCase().includes(term.toLowerCase())
  );
}

// deleteProduct(id: number): void {
//   const dialogRef = this.dialog.open(ConfirmDialogComponent, {
//     width: '350px',
//     data: {
//       title: 'Eliminar producto',
//       message: '¿Está seguro que desea eliminar este producto? Esta acción no se puede deshacer.'
//     }
//   });

//   dialogRef.afterClosed().subscribe(result => {
//     if (result) {
//       this.productService.delete(id).subscribe({
//         next: () => {
//           this.snackbar.success('Producto eliminado correctamente');
//           this.loadPagedProducts();
//         },
//         error: () => this.snackbar.error('Error al eliminar el producto')
//       });
//     }
//   });
// }

deleteProduct(id: number): void {
  this.dialogService.confirm(
    'Eliminar producto',
    '¿Estás seguro que deseas eliminar este producto? Esta acción no se puede deshacer.'
  ).subscribe(result => {
    if (result) {
      this.productService.delete(id).subscribe({
        next: () => {
          this.snackbar.success('Producto eliminado correctamente');
          this.loadPagedProducts();
        },
        error: () => {
          this.snackbar.error('Error al eliminar el producto');
        }
      });
    }
  });
}


createProduct(): void {
  const dialogRef = this.dialog.open(ProductBuilderComponent, {
    width: '400px',
    data: null,
    enterAnimationDuration: '250ms',
    exitAnimationDuration: '200ms'
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      console.log('Crear producto con:', result);
      // TODO: Llamar servicio y refrescar listado
      this.productService.create(result).subscribe(() => this.loadPagedProducts());
    }
  });
}

// editProduct(product: ProductDto): void {
//   const dialogRef = this.dialog.open(ProductBuilderComponent, {
//     width: '400px',
//     data: product
//   });

//   dialogRef.afterClosed().subscribe(result => {
//     if (result) {
//       console.log('Actualizar producto con:', result);
//       // TODO: Llamar servicio y refrescar listado
//       this.productService.update(result.id, result).subscribe(() => this.loadPagedProducts());
//     }
//   });
// }

// paginate(event: PageEvent): void {
//   this.pageIndex = event.pageIndex;
//   this.pageSize = event.pageSize;
//   this.updatePaginatedProducts();
// }

updatePaginatedProducts(): void {
  const start = this.pageIndex * this.pageSize;
  const end = start + this.pageSize;
  this.paginatedProducts = this.filteredProducts.slice(start, end);
}

toggleEnable(product: ProductDto) {
  const newStatus = !product.enable;
  const action = newStatus ? 'habilitar' : 'deshabilitar';

  if (confirm(`¿Estás seguro de que querés ${action} este producto y todas sus relaciones?`)) {
    this.productService.setProductEnable(product.id, newStatus).subscribe({
      next: () => {
        this.snackbar.success(`Producto ${action}o correctamente.`);
        this.loadPagedProducts();
      },
      error: () => {
        this.snackbar.error(`No se pudo ${action} el producto.`);
      }
    });
  }
}



}
