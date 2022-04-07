import { Injectable } from '@angular/core';
import {TokenService} from "./token.service";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private tokenService: TokenService, private jwtHelperService: JwtHelperService ) { }

  getAccountData() : any{
    let token = this.tokenService.getToken();
    if (token == null)
      return;

    const decodedToken = this.jwtHelperService.decodeToken(token);

    return decodedToken;
  }

  authenticated() : boolean {
    let token = this.tokenService.getToken();
    if (token == null)
      return false;

    if(this.jwtHelperService.isTokenExpired(token))
      return false;

    return true;
  }
}
