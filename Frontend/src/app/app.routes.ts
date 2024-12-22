import { Routes } from '@angular/router';
import { PathConstant } from './core/constants/path.constant';
import { authGuardFn } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { MyLinksComponent } from './features/dashboard/pages/my-links/my-links.component';
import { RecommendationsComponent } from './features/dashboard/pages/recommendations/recommendations.component';
import { SettingsComponent } from './features/dashboard/pages/settings/settings.component';
import { SharedComponent } from './features/dashboard/pages/shared/shared.component';
import { IndexComponent } from './features/index/index.component';

export const routes: Routes = [
  { path: PathConstant.INDEX, component: IndexComponent, pathMatch: 'full' },
  { path: PathConstant.LOGIN, component: LoginComponent },
  { path: PathConstant.REGISTER, component: RegisterComponent },
  {
    path: PathConstant.DASHBOARD,
    component: DashboardComponent,
    canActivate: [authGuardFn],
    children: [
      { path: PathConstant.MY_LINKS, component: MyLinksComponent },
      { path: PathConstant.SHARED, component: SharedComponent },
      {
        path: PathConstant.RECOMMENDATIONS,
        component: RecommendationsComponent,
      },
      { path: PathConstant.SETTINGS, component: SettingsComponent },
      { path: '**', redirectTo: PathConstant.MY_LINKS },
    ],
  },
  { path: '**', redirectTo: PathConstant.INDEX },
];
