import { TestBed } from '@angular/core/testing';

import { ReferenceDataStateService } from './reference-data-state.service';

describe('ReferenceDataStateService', () => {
  let service: ReferenceDataStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReferenceDataStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
