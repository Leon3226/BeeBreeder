import {Component, Input, OnInit} from '@angular/core';
import {BeeStack} from "../../model/bee/bee-stack";

@Component({
  selector: 'app-bee-list',
  templateUrl: './bee-list.component.html',
  styleUrls: ['./bee-list.component.css']
})
export class BeeListComponent implements OnInit {

  @Input("bees")
  bees: BeeStack[] = [];

  constructor() {
  }

  ngOnInit(): void {
  }

}
