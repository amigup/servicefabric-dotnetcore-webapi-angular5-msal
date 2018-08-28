import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpModule} from '@angular/http';
import {RouterModule} from '@angular/router';
import {AppComponent} from './app.component';
import {HomeComponent} from './home/home.component'
import {ProductComponent} from './product/product.component'
import {ErrorComponent} from './error.component'
import {ProductDetailComponent} from './product/product-detail.component'
import {ProductService} from './product/product.service';
import {appRoutes} from './app.routes';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {MsalModule} from "@azure/msal-angular";
import { MsalInterceptor} from "@azure/msal-angular";
import {LogLevel} from "msal";
import { TodoListComponent } from './todo-list/todo-list.component';
import {TodoListService} from "./todo-list/todo-list.service";
import {UserDataComponent} from "./user-data/user-data.component";
import { HttpServiceHelper } from './common/HttpServiceHelper';
import {environment} from '../environments/environment'

export function loggerCallback(logLevel, message, piiEnabled) {
  console.log("client logging" + message);
}

export const protectedResourceMap: Map<string, Array<string>> = new Map<string, Array<string>>();
protectedResourceMap.set(environment.apiBaseUrl + "claims", [environment.adalConfig.scope]);
protectedResourceMap.set(environment.apiBaseUrl + "api/user", [environment.adalConfig.scope]);
protectedResourceMap.set(environment.graphApiUrl + "v1.0/me", [environment.adalConfig.graphscope]);

@NgModule({
  declarations: [
    AppComponent, HomeComponent, ProductComponent, ErrorComponent, ProductDetailComponent, TodoListComponent, UserDataComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),  // RouterModule.forRoot(appRoutes,{useHash:true}) for hashbang url
    MsalModule.forRoot({
        clientID: environment.adalConfig.clientId,
        authority: environment.adalConfig.authority,
        validateAuthority: true,
        redirectUri: environment.adalConfig.redirectUri,
        cacheLocation : "localStorage",
        postLogoutRedirectUri: environment.adalConfig.postLogoutRedirectUri,
        navigateToLoginRequestUrl: true,
        popUp: true,
        consentScopes: [ environment.adalConfig.graphscope, environment.adalConfig.scope],
        unprotectedResources: ["https://www.microsoft.com/en-us/"],
        protectedResourceMap: protectedResourceMap,
        logger: loggerCallback,
        correlationId: '1234',
        level: LogLevel.Info,
        piiLoggingEnabled: true
      }
    ),
  ],
  providers: [ProductService, TodoListService, HttpServiceHelper,
     {provide: HTTP_INTERCEPTORS, useClass: MsalInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
