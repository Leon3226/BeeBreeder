import { Component, Input, OnInit } from '@angular/core';
import { Inventory } from 'src/app/model/apiary/inventory';
import { TransposerWithInventories } from 'src/app/model/apiary/transposer-with-inventories';

@Component({
  selector: 'app-trasposer-inventories-display',
  templateUrl: './trasposer-inventories-display.component.html',
  styleUrls: ['./trasposer-inventories-display.component.css']
})
export class TrasposerInventoriesDisplayComponent implements OnInit {

  @Input()
  transposers : TransposerWithInventories[];

  places: (number|undefined)[][] =
  [
    [1,2, undefined],
    [3,-1, 4],
    [undefined,5, 0],
  ]

  constructor() { }

  ngOnInit(): void {

  }
}
