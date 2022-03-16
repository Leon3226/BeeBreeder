import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BeeListComponent } from './bee-list.component';

describe('BeeListComponent', () => {
  let component: BeeListComponent;
  let fixture: ComponentFixture<BeeListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BeeListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BeeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
