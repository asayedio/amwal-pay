import { Component, Inject } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass'],
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';

  constructor(
    @Inject(AuthService) private authService: AuthService,
    private router: Router
  ) {}

  login() {
    this.authService.login(this.username, this.password).subscribe(
      (isAuthenticated: boolean) => {
        if (isAuthenticated) {
          this.router.navigate(['/transaction']);
        } else {
          this.errorMessage = 'Login failed. Please check your credentials.';
        }
      },
      (error) => {
        this.errorMessage = 'An error occurred during login.';
      }
    );
  }
}
