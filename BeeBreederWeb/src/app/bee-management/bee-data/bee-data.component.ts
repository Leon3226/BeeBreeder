import {Component, Input, OnInit} from '@angular/core';
import {Bee} from "../../../model/bee/bee";

@Component({
  selector: 'app-bee-data',
  templateUrl: './bee-data.component.html',
  styleUrls: ['./bee-data.component.css']
})
export class BeeDataComponent implements OnInit {

  @Input("bee")
  bee: Bee = new Bee();
  constructor() { }

  ngOnInit(): void {
  }

}
