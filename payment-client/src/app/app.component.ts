import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass'],
})
export class AppComponent {
  constructor(private authService: AuthService, private router: Router) {}
  title = 'Amwal Pay';
  // Check if the user is authenticated
  isAuthenticated(): boolean {
    return this.authService.isLoggedIn();
  }

  // Handle logout
  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
