import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

  loading = true;

ngOnInit(): void {
  setTimeout(() => this.loading = false, 1500);
}

}
