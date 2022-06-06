import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { BeeListComponent } from './bee-management/bee-list/bee-list.component';
import { BeeDataComponent } from './bee-management/bee-data/bee-data.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {AuthInterceptor} from "./auth.interceptor";
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { SecureComponent } from './auth/secure/secure.component';
import { NotFoundComponent } from './auth/not-found/not-found.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {RouterModule} from "@angular/router";
import { BeeControlPanelComponent } from './bee-management/bee-control-panel/bee-control-panel.component';
import {FormBuilder, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {JwtModule} from "@auth0/angular-jwt";
import { ApiariesSelectorComponent } from './bee-management/apiaries-selector/apiaries-selector.component';
import { ComputersListComponent } from './bee-management/computers-list/computers-list.component';
import { TransposerInventoriesDisplayComponent } from './bee-management/bee-control-panel/transposer-inventories-display/transposer-inventories-display.component';
import { TrasposerInventoriesDisplayComponent } from './bee-management/trasposer-inventories-display/trasposer-inventories-display.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    BeeListComponent,
    BeeDataComponent,
    LoginComponent,
    RegisterComponent,
    SecureComponent,
    NotFoundComponent,
    BeeControlPanelComponent,
    ApiariesSelectorComponent,
    ComputersListComponent,
    TransposerInventoriesDisplayComponent,
    TrasposerInventoriesDisplayComponent
  ],
  imports: [
    ReactiveFormsModule,
    AppRoutingModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    JwtModule.forRoot({
      config: {
        allowedDomains: ["example.com"],
        disallowedRoutes: ["http://example.com/examplebadroute/"],
      },
    }),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
