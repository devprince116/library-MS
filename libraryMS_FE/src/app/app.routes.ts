import { Routes } from '@angular/router';

import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';
import { BookCatalogComponent } from './components/book-catalog/catalog.component';
import { UserDashboardComponent } from './components/dashboard/user/user-dashboard.component';
import { OverdueReportsComponent } from './components/due-report/due-report.component';
import { AdminDashboardComponent } from './components/dashboard/admin/admin-dashboard.component';
import { LoginComponent } from './components/auth/login/login.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { userGuard } from './guards/user.guard';

export const routes: Routes = [
  { path: '', component: BookCatalogComponent },
  { path: 'login', component: LoginComponent },
  { path: "signup", component: SignupComponent },
  { path: 'dashboard', component: UserDashboardComponent, canActivate: [authGuard, userGuard] },
  { path: 'overdue', component: OverdueReportsComponent, canActivate: [authGuard, adminGuard] },
  { path: 'admin', component: AdminDashboardComponent, canActivate: [authGuard, adminGuard] },
  { path: '**', redirectTo: '' },
];