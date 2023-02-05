import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Computer} from "../model/property/computer";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ComputersService {

  constructor(private http: HttpClient) { }

  public getComputers() : Observable<Computer[]> {
    return this.http.get<any>("http://localhost:5001/api/Computers")
  }

  public getConnected(identifier: string) : Observable<any> {
    return this.http.get<any>(`http://localhost:5001/api/Computers/connect_check/${identifier}`)
  }

  public addComputer(computer: Computer) : Observable<any> {
    return this.http.post<any>("http://localhost:5001/api/Computers", computer);
  }

  public editComputer(id : number, computer: Computer) : Observable<any> {
    return this.http.put<any>(`http://localhost:5001/api/Computers/${id}`, computer);
  }

  public deleteComputer(id : number) : Observable<any> {
    return this.http.delete<any>(`http://localhost:5001/api/Computers/${id}`);
  }
}
