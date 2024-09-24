import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private cookieName = 'isAuthenticated';

  constructor(private cookieService: CookieService) {}

  login(username: string, password: string): Observable<boolean> {
    if (username === 'admin' && password === '123') {
      this.setAuthCookie(true);
      return of(true);
    } else {
      this.setAuthCookie(false);
      return of(false);
    }
  }

  logout() {
    this.setAuthCookie(false);
  }

  isLoggedIn(): boolean {
    return this.getAuthCookie();
  }

  private setAuthCookie(isAuthenticated: boolean) {
    const value = isAuthenticated ? 'true' : 'false';
    this.cookieService.set(this.cookieName, value, 7);
  }

  private getAuthCookie(): boolean {
    return this.cookieService.get(this.cookieName) === 'true';
  }
}
