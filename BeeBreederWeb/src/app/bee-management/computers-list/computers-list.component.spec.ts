import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComputersListComponent } from './computers-list.component';

describe('ComputersListComponent', () => {
  let component: ComputersListComponent;
  let fixture: ComponentFixture<ComputersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ComputersListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComputersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
