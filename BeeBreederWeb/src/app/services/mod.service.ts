import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Mod } from '../model/property/mod';

@Injectable({
  providedIn: 'root'
})
export class ModService {

  constructor(private http: HttpClient) { }

  public getMods() : Observable<Mod[]> {
    return this.http.get<any>("http://localhost:5001/api/Mods")
  }
}
