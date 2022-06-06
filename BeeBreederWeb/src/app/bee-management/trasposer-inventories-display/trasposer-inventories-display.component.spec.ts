import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrasposerInventoriesDisplayComponent } from './trasposer-inventories-display.component';

describe('TrasposerInventoriesDisplayComponent', () => {
  let component: TrasposerInventoriesDisplayComponent;
  let fixture: ComponentFixture<TrasposerInventoriesDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TrasposerInventoriesDisplayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TrasposerInventoriesDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
