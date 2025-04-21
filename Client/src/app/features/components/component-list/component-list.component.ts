import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ComponentBuilderService } from '../services/component-builder.service';
import { SnackbarService } from 'shared/snackbar.service';
import { DialogService } from 'shared/service/dialog.service';
import { Router } from '@angular/router';
import { ComponentDto } from '../models/component.dto';
import { PaginationQueryDto } from 'core/dtos/PaginationQueryDto';

@Component({
  selector: 'app-component-list',
  standalone:false,
  templateUrl: './component-list.component.html',
  styleUrls: ['./component-list.component.scss']
})
export class ComponentListComponent implements OnInit {
  components: ComponentDto[] = [];
  filteredComponents: ComponentDto[] = [];
  paginatedComponents: ComponentDto[] = [];
  totalItems = 0;
  pageIndex = 0;
  pageSize = 10;
  isLoading = true;
  searchText = '';

  dataSource = new MatTableDataSource<ComponentDto>();
  displayedColumns: string[] = ['name','description', 'code', 'unitSymbol', 'unitCost', 'category','componentTypeName', 'actions'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private componentService: ComponentBuilderService,
    private snackbar: SnackbarService,
    private dialogService: DialogService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras.state as { justSaved?: boolean };
  
    if (state?.justSaved) {
      setTimeout(() => {
        this.snackbar.success('Producto guardado exitosamente');
      }, 100);
    }

    this.loadComponents();
  }

  duplicateComponent(id: number): void {
    this.dialogService
    .confirm('Duplicar componente', '¿Deseás duplicar este componente y usarlo como base para uno nuevo?')
    .subscribe(result => {
      if (result) {
        this.router.navigate(['/components/builder', id], {
          queryParams: { duplicate: true }
        });
      }
    });
  }

  loadComponents(): void {
    this.isLoading = true;
    const query: PaginationQueryDto = {
      pageIndex: this.pageIndex,
      pageSize: this.pageSize,
      search: this.searchText,
      enable: true
    };
  
    this.componentService.getPagedComponents(query).subscribe({
      next: result => {
        this.filteredComponents = result.items;
        this.totalItems = result.totalItems;
        this.updatePaginatedComponents();
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Error loading products:', err);
      }
    });
  }

  applyFilter(term: string) {
    this.filteredComponents = this.components.filter(c =>
      c.name.toLowerCase().includes(term.toLowerCase())
    );
    this.pageIndex = 0;
    this.updatePaginatedComponents();
  }

  paginate(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.updatePaginatedComponents();
  }

  updatePaginatedComponents(): void {
     const start = this.pageIndex * this.pageSize;
     const end = start + this.pageSize;
     this.paginatedComponents = this.filteredComponents.slice(start, end);
  }

  toggleEnable(component: ComponentDto): void {
    const newStatus = !component.enable;
    const action = newStatus ? 'habilitar' : 'deshabilitar';

    this.dialogService.confirm('Confirmar', `¿Desea ${action} este componente?`).subscribe(result => {
      if (result) {
        this.componentService.setEnable(component.id, newStatus).subscribe({
          next: () => {
            this.snackbar.success(`Componente ${action}o correctamente.`);
            this.loadComponents();
          },
          error: () => this.snackbar.error(`No se pudo ${action} el componente.`)
        });
      }
    });
  }

  deleteComponent(id: number): void {
    this.dialogService.confirm('Eliminar', '¿Está seguro que desea eliminar este componente?').subscribe(result => {
      if (result) {
        this.componentService.delete(id).subscribe({
          next: () => {
            this.snackbar.success('Componente eliminado correctamente');
            this.loadComponents();
          },
          error: () => this.snackbar.error('Error al eliminar el componente')
        });
      }
    });
  }
}
