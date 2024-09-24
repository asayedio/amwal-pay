import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { AuthService } from './services/auth.service';
import { RouterModule } from '@angular/router';
import { TransactionComponent } from './components/transaction/transaction.component';
import { TransactionService } from './services/transaction.service';
import { CookieService } from 'ngx-cookie-service';

@NgModule({
  declarations: [AppComponent, LoginComponent, TransactionComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'transaction', component: TransactionComponent },
      { path: 'login', component: LoginComponent },
    ]),
  ],
  providers: [AuthService, TransactionService, CookieService],
  bootstrap: [AppComponent],
})
export class AppModule {}
