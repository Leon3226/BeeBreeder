import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {BeeStack} from "../../../model/bee/bee-stack";
import {Bee} from "../../../model/bee/bee";

@Component({
  selector: 'app-bee-list',
  templateUrl: './bee-list.component.html',
  styleUrls: ['./bee-list.component.css']
})
export class BeeListComponent implements OnInit {

  @Input("bees")
  bees: BeeStack[] = [];
  selectedBee: Bee = new Bee();

  @Output()
  selectedBeeChanged = new EventEmitter<Bee>();

  onBeeSelection(bee : Bee){
    this.selectedBee = bee;
    this.selectedBeeChanged.emit(this.selectedBee);
  }

  constructor() {
  }

  ngOnInit(): void {
  }

}
