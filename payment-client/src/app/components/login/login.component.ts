import { Component, Inject } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(@Inject(AuthService) private authService: AuthService) {}

  login() {
    this.authService
      .login(this.username, this.password)
      .subscribe((isAuthenticated: boolean) => {
        // Explicitly set the type of 'isAuthenticated'
        if (isAuthenticated) {
          console.log('Login successful');
          // Proceed with further actions after login, such as redirecting the user
        } else {
          console.log('Login failed');
        }
      });
  }
}
