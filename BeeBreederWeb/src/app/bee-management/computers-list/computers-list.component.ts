import {Component, Input, OnInit} from '@angular/core';
import {Computer} from "../../model/property/computer";
import { fromEvent, debounceTime, distinctUntilChanged } from 'rxjs';
import {ComputersService} from "../../services/computers.service";
import {Observable} from "rxjs";
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-computers-list',
  templateUrl: './computers-list.component.html',
  styleUrls: ['./computers-list.component.css']
})
export class ComputersListComponent implements OnInit {

  request : Observable<Computer[]>;
  computers: Computer[];
  connectedCheck : any = {} //TODO: Maybe specify a type later
  
  openedTab : number = 1;

  addCompForm = this.fb.group({
    name: ['', Validators.required],
    identifier: ['', Validators.required],
    description : ['', Validators.maxLength(300)],
    mods: ['']
  })

  editCompForm = this.fb.group({
    id: ['', Validators.required],
    name: ['', Validators.required],
    identifier: ['', Validators.required],
    description : ['', Validators.maxLength(300)],
    mods: ['']
  })

  addInProgress : boolean = false;
  checkingComputerInProgress : boolean = false;
  deletingInProgress : boolean = false;
  editInProgress : boolean = false;

  constructor(private computersService: ComputersService, private fb: FormBuilder) {
    this.request = this.computersService.getComputers();
    this.editCompForm.controls['identifier'].valueChanges
      .pipe(debounceTime(500), distinctUntilChanged())
      .subscribe(text => {
        this.checkingComputerInProgress = true;
        this.computersService.getConnected(text).subscribe((check) => {
          this.connectedCheck = check 
          this.checkingComputerInProgress = false;
        }, (error) => {this.checkingComputerInProgress = false;});
      })

      this.addCompForm.controls['identifier'].valueChanges
      .pipe(debounceTime(500), distinctUntilChanged())
      .subscribe(text => {
        this.checkingComputerInProgress = true;
        this.computersService.getConnected(text).subscribe((check) => {
          this.connectedCheck = check 
          this.checkingComputerInProgress = false;
        }, (error) => {this.checkingComputerInProgress = false;});
      })
  }

  ngOnInit(): void {
    this.updateComputers();
  }

  submitAdd(){
     if (this.addCompForm.invalid || !this.connectedCheck.allowedToAdd) return;

     let newComputer = this.addCompForm.value as Computer;
     this.addInProgress = true;  
     this.computersService.addComputer(newComputer).subscribe(x => {
       this.updateComputers();
       this.backToList();
       this.addInProgress = false;
     }, (error) => {this.addInProgress = false;});
  }

  submitEdit(){
    if (this.editCompForm.invalid) return;

    let newComputer = this.editCompForm.value as Computer;
    this.editInProgress = true;  
    this.computersService.editComputer(newComputer.id, newComputer).subscribe(x => {
      this.updateComputers();
      this.backToList();
      this.addInProgress = false;
    }, (error) => {this.editInProgress = false;});
 }

  updateComputers(){
    this.request.subscribe(response => {
      this.computers = response;
    })
  }

  deleteComputer(id : number){
    this.computersService.deleteComputer(id).subscribe(x => this.updateComputers());
  }

  editComputer(computer : Computer){
     this.editCompForm.controls['id'].setValue(computer.id);
     this.editCompForm.controls['name'].setValue(computer.name);
     this.editCompForm.controls['identifier'].setValue(computer.identifier);
     this.editCompForm.controls['description'].setValue(computer.description);
    this.openedTab = 2
  }

  addComputer(){
    this.openedTab = 0
  }

  backToList(){
    this.openedTab = 1
  }

}
