import { TestBed } from '@angular/core/testing';

import { ApiaryService } from './apiary.service';

describe('ApiaryService', () => {
  let service: ApiaryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApiaryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
