import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Inventory } from '../model/apiary/inventory';

@Injectable({
  providedIn: 'root'
})
export class InventoriesService {

  constructor(private http: HttpClient) { }

  public getInventories(computerId: number, transposerId: string) : Observable<Inventory[]> {
    return this.http.get<any>(`http://localhost:5001/api/computers/${computerId}/transposers/${transposerId}/inventories/`)
  }

  public getInventory(computerId: number, transposerId: string, side: number) : Observable<Inventory> {
    return this.http.get<any>(`http://localhost:5001/api/computers/${computerId}/transposers/${transposerId}/inventories/${side}`)
  }

  public addInventory(computerId: number, transposerId: string, side: number, transposer: Inventory) : Observable<any> {
    return this.http.post<any>(`http://localhost:5001/api/computers/${computerId}/transposers/${transposerId}/inventories/${side}`, transposer);
  }

  public editInventory(computerId: number, transposerId: string, side: number, transposer: Inventory) : Observable<any> {
    return this.http.put<any>(`http://localhost:5001/api/computers/${computerId}/transposers/${transposerId}/inventories/${side}`, transposer);
  }

  public deleteInventory(computerId: number, transposerId: string, side: number) : Observable<any> {
    return this.http.delete<any>(`http://localhost:5001/api/computers/${computerId}/transposers/${transposerId}/inventories/${side}`);
  }
}
