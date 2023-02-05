import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransposerInventoryIconComponent } from './transposer-inventory-icon.component';

describe('TransposerInventoryIconComponent', () => {
  let component: TransposerInventoryIconComponent;
  let fixture: ComponentFixture<TransposerInventoryIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TransposerInventoryIconComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TransposerInventoryIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
