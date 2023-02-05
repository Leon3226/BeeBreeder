import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Inventory } from '../model/apiary/inventory';
import { Transposer } from '../model/apiary/transposer';
import { TransposerWithInventories } from '../model/apiary/transposer-with-inventories';

@Injectable({
  providedIn: 'root'
})
export class TransposersService {

  constructor(private http: HttpClient) { }

  public getGameTransposers(computer: string) : Observable<string[]> {
    return this.http.get<any>(`http://localhost:5001/api/InGameData/${computer}`)
  }

  public getGameTransposer(computer: string, transposer: string) : Observable<Inventory[]> {
    return this.http.get<any>(`http://localhost:5001/api/InGameData/${computer}/${transposer}`)
  }

  public getTransposers(computerId: number) : Observable<Transposer[]> {
    return this.http.get<any>(`http://localhost:5001/api/computers/${computerId}/transposers`)
  }

  public getTransposer(computerId: number, id: string) : Observable<Transposer> {
    return this.http.get<any>(`http://localhost:5001/api/computers/${computerId}/transposers/${id}`)
  }

  public addTransposer(computerId: number, transposer: Transposer) : Observable<any> {
    return this.http.post<any>(`http://localhost:5001/api/computers/${computerId}/transposers`, transposer);
  }

  public editTransposer(computerId: number, id: string, transposer: Transposer) : Observable<any> {
    return this.http.put<any>(`http://localhost:5001/api/computers/${computerId}/transposers/${id}`, transposer);
  }

  public deleteTransposer(computerId: number, id : string) : Observable<any> {
    return this.http.delete<any>(`http://localhost:5001/api/computers/${computerId}/transposers/${id}`);
  }
}
