import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Computer} from "../../model/property/computer";
import {Apiary} from "../../model/property/apiary";

@Injectable({
  providedIn: 'root'
})
export class ApiaryService {

  constructor(private http: HttpClient) { }

  public getApiaries() : Observable<Apiary[]> {
    return this.http.get<any>("http://localhost:5001/api/Apiaries")
  }

  public addApiary(apiary: Apiary) : Observable<any> {
    return this.http.post<any>("http://localhost:5001/api/Apiaries", apiary);
  }

  public deleteApiary(id : number) : Observable<any> {
    return this.http.delete<any>(`http://localhost:5001/api/Apiaries/${id}`);
  }
}
