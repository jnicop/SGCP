import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { AuthGuard } from './core/auth.guard'; // ✅ Import corregido
import { LoginComponent } from 'auth/login/login.component';

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./features/dashboard/dashboard.module').then(m => m.DashboardModule)
      },
      {
        path: 'products',
        loadChildren: () =>
          import('./features/products/products.module').then(m => m.ProductsModule)
      },
      {
        path: 'components',
        loadChildren: () =>
          import('./features/components/components.module').then(m => m.ComponentsModule)
      }
    ]
  },
{
  path: 'login',
  component: LoginComponent
},
  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
