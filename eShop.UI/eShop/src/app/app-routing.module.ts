import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './helpers/auth-guard';
import { role } from './models/role';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { ModeratorComponent } from './pages/moderator/moderator.component';
import { RegisterComponent } from './pages/register/register.component';

const routes: Routes = [
  {
    path: 'moderator',
    component: ModeratorComponent,
    canActivate: [AuthGuard],
    data: { roles: [role.Moderator] },
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
