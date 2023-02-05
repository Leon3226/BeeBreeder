import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Computer } from 'src/app/model/property/computer';
import { Mod } from 'src/app/model/property/mod';
import { ComputersService } from 'src/app/services/computers.service';
import { ModService } from 'src/app/services/mod.service';
import { Apiary } from '../../model/property/apiary';
import { ApiaryService } from '../../services/apiary.service';

@Component({
  selector: 'app-apiaries-selector',
  templateUrl: './apiaries-selector.component.html',
  styleUrls: ['./apiaries-selector.component.css'],
})
export class ApiariesSelectorComponent implements OnInit {
  @Output() switchApiaryEvent = new EventEmitter<number>();

  apiaries: Apiary[];
  computersOptions: any[] = [];

  openedTab: number = 1;

  addInProgress: boolean = false;
  deletingInProgress: boolean = false;
  editInProgress: boolean = false;

  selectionListEnabled: boolean = false;

  addModForm = this.fb.group({
    name: ['', Validators.required],
    description: ['', Validators.maxLength(300)],
    mods: [''],
  });

  attachComputerForm = this.fb.group({
    apiaryId: ['', Validators.required],
    computerId: ['', Validators.required],
  });

  editModForm = this.fb.group({
    id: ['', Validators.required],
    name: ['', Validators.required],
    description: ['', Validators.maxLength(300)],
    mods: [''],
  });

  modOptions: Mod[] = [];

  constructor(
    private apiaryService: ApiaryService,
    private computersService: ComputersService,
    private modService: ModService,
    private fb: FormBuilder
  ) {}

  submitAdd() {
    if (this.addModForm.invalid) return;

    let newApiary = this.addModForm.value as Apiary;
    this.addInProgress = true;
    this.apiaryService.addApiary(newApiary).subscribe(
      (x) => {
        this.updateApiaries();
        this.backToList();
        this.addInProgress = false;
      },
      (error) => {
        this.addInProgress = false;
      }
    );
  }

  switchSelectionListEnabled() {
    this.updateComputersList();
    this.selectionListEnabled = !this.selectionListEnabled;
  }

  updateComputersList() {
    this.computersService.getComputers().subscribe((x) => {
      this.computersOptions = [];
      this.computersOptions.push({
        name: 'Unoccupied',
        computers: x.filter((comp) => comp.apiaryId == null),
      });
      this.computersOptions.push({
        name: 'Occupied',
        computers: x.filter((comp) => comp.apiaryId != null),
      });
    });
  }

  onSelectAddComputer(apiaryId: number, computerId: number) {
    this.selectionListEnabled = false;
    this.apiaryService
      .attachComputer(apiaryId, computerId)
      .subscribe((x) => this.updateApiaries());
  }

  onSelectRemoveComputer(apiaryId: number, computerId: number) {
    this.apiaryService
      .detachComputer(apiaryId, computerId)
      .subscribe((x) => this.updateApiaries());
  }

  submitEdit() {
    if (this.editModForm.invalid) return;

    let editedApiary = this.editModForm.value as Apiary;
    this.editInProgress = true;
    this.apiaryService.editApiary(editedApiary.id, editedApiary).subscribe(
      (x) => {
        this.updateApiaries();
        this.backToList();
        this.editInProgress = false;
      },
      (error) => {
        this.editInProgress = false;
      }
    );
  }

  ngOnInit(): void {
    this.updateApiaries();
    this.modService.getMods().subscribe((mods) => {
      this.modOptions = mods;
    });
  }

  deleteApiary(id: number) {
    this.apiaryService.deleteApiary(id).subscribe((x) => this.updateApiaries());
  }

  editApary(apiary: Apiary) {
    this.editModForm.controls['id'].setValue(apiary.id);
    this.editModForm.controls['name'].setValue(apiary.name);
    this.editModForm.controls['description'].setValue(apiary.description);
    this.editModForm.controls['mods'].setValue(apiary.mods);
    this.openedTab = 2;
  }

  addApiary() {
    this.openedTab = 0;
  }

  backToList() {
    this.openedTab = 1;
  }

  updateApiaries(): void {
    this.apiaryService.getApiaries().subscribe((response) => {
      this.apiaries = response;
    });
  }

  switchToThisApiary(id: number) {
    this.switchApiaryEvent.emit(id);
  }
}
