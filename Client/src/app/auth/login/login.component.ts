import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'core/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: false
})
export class LoginComponent {
  username = '';
  password = '';
  remember = false;
  loading = false;
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    const savedUsername = localStorage.getItem('rememberedUser');
    if (savedUsername) {
      this.username = savedUsername;
      this.remember = true;
    }
  }

  async onSubmit() {
    this.error = '';
    this.loading = true;
  
    try {
      await this.authService.login(this.username, this.password);
  
      if (this.remember) {
        localStorage.setItem('rememberedUser', this.username);
      } else {
        localStorage.removeItem('rememberedUser');
      }
  
      // ✅ Redirigir al returnUrl si existe, sino al dashboard
      const returnUrl = localStorage.getItem('returnUrl');
      if (returnUrl) {
        localStorage.removeItem('returnUrl');
        this.router.navigateByUrl(returnUrl);
      } else {
        this.router.navigate(['/dashboard']);
      }
  
    } catch (err: any) {
      this.error = err.error?.message || 'Error al iniciar sesión';
    } finally {
      this.loading = false;
    }
  }
}
