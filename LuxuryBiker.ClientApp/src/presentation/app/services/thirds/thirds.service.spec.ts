import { TestBed } from '@angular/core/testing';

import { ThirdsService } from './thirds.service';

describe('ThirdsService', () => {
  let service: ThirdsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ThirdsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
