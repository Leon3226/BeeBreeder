import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Computer} from "../../model/property/computer";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ComputersService {

  constructor(private http: HttpClient) { }

  public getComputers() : Observable<Computer[]> {
    return this.http.get<any>("http://localhost:5001/ApiaryData/Computers")
  }
}
