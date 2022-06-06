import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApiariesSelectorComponent } from './apiaries-selector.component';

describe('ApiariesSelectorComponent', () => {
  let component: ApiariesSelectorComponent;
  let fixture: ComponentFixture<ApiariesSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApiariesSelectorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApiariesSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
