import { TestBed } from '@angular/core/testing';

import { SearchCriteriaStateService } from './search-criteria-state.service';

describe('SearchCriteriaStateService', () => {
  let service: SearchCriteriaStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SearchCriteriaStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
