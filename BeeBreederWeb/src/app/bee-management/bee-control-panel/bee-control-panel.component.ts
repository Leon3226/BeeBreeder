import { Component, OnInit } from '@angular/core';
import {BeeStack} from "../../../model/bee/bee-stack";
import {Bee} from "../../../model/bee/bee";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-bee-control-panel',
  templateUrl: './bee-control-panel.component.html',
  styleUrls: ['./bee-control-panel.component.css']
})
export class BeeControlPanelComponent implements OnInit {

  constructor(private http: HttpClient) { }

  bees: BeeStack[] = [];
  selectedBee : Bee = new Bee();
  isLoadingData : boolean = false;

  onBeeSelected(bee: Bee) : void{
    this.selectedBee = bee;
  }

  ngOnInit(): void {
    this.isLoadingData = true;
    this.http.get<any>("http://localhost:5001/ApiaryData").subscribe(response => {
        this.bees = response.bees;
        this.isLoadingData = false;
    })
  }
}
