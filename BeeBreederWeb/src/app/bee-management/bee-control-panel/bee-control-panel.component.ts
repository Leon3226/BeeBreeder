import { Component, OnInit } from '@angular/core';
import {BeeStack} from "../../model/bee/bee-stack";
import {Bee} from "../../model/bee/bee";
import {HttpClient} from "@angular/common/http";
import {Apiary} from "../../model/property/apiary";
import {Computer} from "../../model/property/computer";
import {ComputersService} from "../../services/computers.service";
import { TransposersService } from 'src/app/services/transposers.service';
import { ApiaryService } from 'src/app/services/apiary.service';
import { TransposerWithInventories } from 'src/app/model/apiary/transposer-with-inventories';
import { Inventory } from 'src/app/model/apiary/inventory';
import { combineLatest, forkJoin } from 'rxjs';
import { InventoriesService } from 'src/app/services/inventories.service';

@Component({
  selector: 'app-bee-control-panel',
  templateUrl: './bee-control-panel.component.html',
  styleUrls: ['./bee-control-panel.component.css']
})
export class BeeControlPanelComponent implements OnInit {

  constructor(private http: HttpClient,
    private apiaryService: ApiaryService,
    private computersService: ComputersService,
    private inventoriesService: InventoriesService,
    private transposersService: TransposersService) { }

  computers: Computer[] = [];
  transposers: TransposerWithInventories[] = [];
  bees: BeeStack[] = [];
  selectedBee : Bee = new Bee();
  isLoadingData : boolean = false;

  onBeeSelected(bee: Bee) : void{
    this.selectedBee = bee;
  }

  ngOnInit(): void {
    this.isLoadingData = true;
    this.http.get<any>("http://localhost:5001/api/InGameData").subscribe(response => {
        this.bees = response.bees;
        this.isLoadingData = false;
    })
  }

  OnApiarySwitched(id: number){
    this.transposers = [];
    this.apiaryService.getApiary(id).subscribe(apiary => {
      apiary.computers.forEach(computer => {
          let transposersInfoObservable = this.transposersService.getTransposers(computer.id);
          let transposersDataObservable = this.transposersService.getGameTransposers(computer.identifier)
          combineLatest([transposersInfoObservable, transposersDataObservable]).subscribe((result) =>
          {
            let info = result[0];
            let data = result[1];

            data.forEach(transposer => {
              let transposerInfo = info.find(x => x.id === transposer) ?? {biome: '', flowers: [], id: transposer, name:'', roofed:false};
              let inventoriesDataObservable = this.transposersService.getGameTransposer(computer.identifier, transposer);
              let inventoriesInfoObservable = this.inventoriesService.getInventories(computer.id, transposer);

              combineLatest([inventoriesDataObservable, inventoriesInfoObservable]).subscribe((inventoriesResult) => {
              let inventoriesData = inventoriesResult[0];
              let inventoriesInfo = inventoriesResult[1];

              inventoriesData.forEach((inventoryData, index) => {
                let inventoryInfo = inventoriesInfo.find(x => x.side == index);
                if (inventoryInfo != null) {
                  inventoriesData[index] = {...inventoryInfo, ...inventoryData};
                }
              })

              this.transposers.push({
                ...transposerInfo,
                inventories: inventoriesData})

            });
          });
      });
    });
  })
  }
}
