import {Component, Input, OnInit} from '@angular/core';
import {Apiary} from "../../../model/property/apiary";
import {ApiaryService} from "../../services/apiary.service";

@Component({
  selector: 'app-apiaries-selector',
  templateUrl: './apiaries-selector.component.html',
  styleUrls: ['./apiaries-selector.component.css']
})
export class ApiariesSelectorComponent implements OnInit {

  apiaries: Apiary[];

  newName : string = ''
  newDescription : string = ''

  addApiary() : any {
    let newApiary = new Apiary();
    newApiary.name = this.newName;
    newApiary.description = this.newDescription;
    this.apiaryService.addApiary(newApiary).subscribe(x => this.updateApiaries());
  }

  deleteApiary(id : number){
    this.apiaryService.deleteApiary(id).subscribe(x => this.updateApiaries());
  }

  constructor(private apiaryService: ApiaryService) { }

  ngOnInit(): void {
    this.updateApiaries();
  }

  updateApiaries() : void{
    this.apiaryService.getApiaries().subscribe(response => {
      this.apiaries = response;
    })
  }

}
