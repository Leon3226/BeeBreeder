import { Component, OnInit } from '@angular/core';
import {AccountService} from "../services/auth/account.service";
import {AccountData} from "../../model/auth/account-data";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  accountData : any;
  loggedIn : boolean;

  constructor(private accountService : AccountService) { }

  ngOnInit(): void {
    this.loggedIn = this.accountService.authenticated();
    this.accountData = this.accountService.getAccountData();
  }

}
