import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private cookieName = 'isAuthenticated';
  private encryptionKeyCookie = 'encryptionKey'; // Name for the encryption key cookie

  constructor(private cookieService: CookieService, private http: HttpClient) {}

  login(username: string, password: string): Observable<boolean> {
    if (username === 'admin' && password === '123') {
      return this.generateEncryptionKey().pipe(
        map((response: any) => {
          this.setAuthCookie(true);
          this.setEncryptionKeyCookie(response.key);
          return true; // Return boolean
        })
      );
    } else {
      this.setAuthCookie(false);
      return of(false); // Return false as Observable<boolean>
    }
  }

  logout() {
    this.setAuthCookie(false);
    this.cookieService.delete(this.encryptionKeyCookie); // Remove the encryption key on logout
  }

  isLoggedIn(): boolean {
    return this.getAuthCookie();
  }

  private setAuthCookie(isAuthenticated: boolean) {
    const value = isAuthenticated ? 'true' : 'false';
    console.log('Setting auth cookie:', value);
    this.cookieService.set(this.cookieName, value, 7); // Cookie expires in 7 days
  }

  private getAuthCookie(): boolean {
    return this.cookieService.get(this.cookieName) === 'true';
  }

  private setEncryptionKeyCookie(key: string) {
    console.log('Setting encryption key cookie:', key);
    this.cookieService.set(this.encryptionKeyCookie, key, 7); // Store the encryption key for 7 days
  }

  private generateEncryptionKey(): Observable<any> {
    const url = 'https://localhost:44387/api/Encryption/GenerateKey';
    return this.http.get<any>(url);
  }

  getEncryptionKey(): string {
    return this.cookieService.get(this.encryptionKeyCookie);
  }
}
