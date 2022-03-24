import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BeeControlPanelComponent } from './bee-control-panel.component';

describe('BeeControlPanelComponent', () => {
  let component: BeeControlPanelComponent;
  let fixture: ComponentFixture<BeeControlPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BeeControlPanelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BeeControlPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
