import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BeeDataComponent } from './bee-data.component';

describe('BeeDataComponent', () => {
  let component: BeeDataComponent;
  let fixture: ComponentFixture<BeeDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BeeDataComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BeeDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
