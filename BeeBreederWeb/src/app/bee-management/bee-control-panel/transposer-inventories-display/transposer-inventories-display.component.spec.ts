import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransposerInventoriesDisplayComponent } from './transposer-inventories-display.component';

describe('TransposerInventoriesDisplayComponent', () => {
  let component: TransposerInventoriesDisplayComponent;
  let fixture: ComponentFixture<TransposerInventoriesDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TransposerInventoriesDisplayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TransposerInventoriesDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
