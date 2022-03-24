import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {SecureComponent} from "./auth/secure/secure.component";
import {NotFoundComponent} from "./auth/not-found/not-found.component";
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./auth/login/login.component";
import {RegisterComponent} from "./auth/register/register.component";
import {AuthGuard} from "./auth.guard";
import {BeeControlPanelComponent} from "./bee-management/bee-control-panel/bee-control-panel.component";

const routes: Routes = [
  { path: '', redirectTo: 'management', pathMatch: 'full' },
  { path: 'secure', canActivate: [ AuthGuard ], component: SecureComponent },
  { path: 'login', component: LoginComponent },
  { path: 'management', component: BeeControlPanelComponent },
  { path: 'register', component: RegisterComponent },
  { path: '404', component: NotFoundComponent },
  { path: '**', redirectTo: '404' }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule,
    RouterModule.forRoot(routes),
    CommonModule
  ],
  exports: [ RouterModule ]
})
export class AppRoutingModule { }
