import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  standalone: false
})
export class NavbarComponent {
  @Output() toggleSidebar = new EventEmitter<void>();
  username = localStorage.getItem('username');

  constructor(private router: Router) {}

  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  onToggleSidebar() {
    this.toggleSidebar.emit();
  }
}
