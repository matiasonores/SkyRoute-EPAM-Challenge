import { TestBed } from '@angular/core/testing';

import { FlightResultsStateService } from './flight-results-state.service';

describe('FlightResultsStateService', () => {
  let service: FlightResultsStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FlightResultsStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
