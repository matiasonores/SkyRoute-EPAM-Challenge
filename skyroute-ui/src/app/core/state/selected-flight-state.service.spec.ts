import { TestBed } from '@angular/core/testing';

import { SelectedFlightStateService } from './selected-flight-state.service';

describe('SelectedFlightStateService', () => {
  let service: SelectedFlightStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SelectedFlightStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
