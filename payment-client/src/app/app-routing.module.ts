import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransactionComponent } from './components/transaction/transaction.component';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { AlreadyAuthGuard } from './guards/already-auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/transaction', pathMatch: 'full' },
  {
    path: 'transaction',
    component: TransactionComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [AlreadyAuthGuard],
  },
  { path: '**', redirectTo: '/transaction' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
