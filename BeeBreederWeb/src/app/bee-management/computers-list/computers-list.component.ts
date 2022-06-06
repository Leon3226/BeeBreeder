import {Component, Input, OnInit} from '@angular/core';
import {Computer} from "../../../model/property/computer";
import {ComputersService} from "../../services/computers.service";
import {Observable} from "rxjs";

@Component({
  selector: 'app-computers-list',
  templateUrl: './computers-list.component.html',
  styleUrls: ['./computers-list.component.css']
})
export class ComputersListComponent implements OnInit {

  request : Observable<Computer[]>;
  computers: Computer[]

  constructor(private computersService: ComputersService) {
    this.request = this.computersService.getComputers();
  }

  ngOnInit(): void {
    this.updateComputers();
  }

  updateComputers(){
    this.request.subscribe(response => {
      this.computers = response;
    })
  }

}
