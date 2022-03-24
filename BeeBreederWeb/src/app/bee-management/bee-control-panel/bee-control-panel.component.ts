import { Component, OnInit } from '@angular/core';
import {BeeStack} from "../../../model/bee/bee-stack";
import {Bee} from "../../../model/bee/bee";

@Component({
  selector: 'app-bee-control-panel',
  templateUrl: './bee-control-panel.component.html',
  styleUrls: ['./bee-control-panel.component.css']
})
export class BeeControlPanelComponent implements OnInit {

  constructor() { }

  bees: BeeStack[] = [];
  selectedBee : Bee = new Bee();
  isLoadingData : boolean = false;

  onBeeSelected(bee: Bee) : void{
    this.selectedBee = bee;
  }

  ngOnInit(): void {
    this.isLoadingData = true;
    const fetchPromise = fetch("http://localhost:5001/ApiaryData");
    fetchPromise.then(response => {
      response.text().then(x => {
        let beeObj = JSON.parse(x);
        this.bees = beeObj.bees;
      })
    }).then(x => this.isLoadingData = false).catch(x => this.isLoadingData = false);
  }
}
