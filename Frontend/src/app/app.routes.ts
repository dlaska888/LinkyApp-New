import { Routes } from '@angular/router';
import { PathConstant } from './core/constants/path.constant';
import { authGuardFn } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { IndexComponent } from './features/index/index.component';
import { DashboardComponent } from './features/dashboard/components/dashboard/dashboard.component';

export const routes: Routes = [
  { path: PathConstant.INDEX, component: IndexComponent, pathMatch: 'full' },
  { path: PathConstant.LOGIN, component: LoginComponent },
  { path: PathConstant.REGISTER, component: RegisterComponent },
  {
    path: PathConstant.DASHBOARD,
    component: DashboardComponent,
    canActivate: [authGuardFn],
  },
  { path: '**', redirectTo: PathConstant.INDEX },
];
