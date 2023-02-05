import { Component, Input, OnInit } from '@angular/core';
import { Inventory } from 'src/app/model/apiary/inventory';
import { ContainerImageService } from 'src/app/services/container-image.service';

@Component({
  selector: 'app-transposer-inventory-icon',
  templateUrl: './transposer-inventory-icon.component.html',
  styleUrls: ['./transposer-inventory-icon.component.css']
})
export class TransposerInventoryIconComponent implements OnInit {

  @Input()
  inventory: Inventory;

  hovering: boolean = false;
  constructor(private containerImageService: ContainerImageService) { }

  ngOnInit(): void {
  }

  getImageLink(containerName: string) : string{
    return this.containerImageService.imageLink(containerName);
  }

  mouseEnter(){
    this.hovering = true;
  }

  mouseLeave(){
    this.hovering = false;
  }
}
