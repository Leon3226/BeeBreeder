import { TestBed } from '@angular/core/testing';

import { TransposersService } from './transposers.service';

describe('TransposersService', () => {
  let service: TransposersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TransposersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
