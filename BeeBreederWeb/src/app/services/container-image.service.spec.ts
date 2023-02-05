import { TestBed } from '@angular/core/testing';

import { ContainerImageService } from './container-image.service';

describe('ContainerImageService', () => {
  let service: ContainerImageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContainerImageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
