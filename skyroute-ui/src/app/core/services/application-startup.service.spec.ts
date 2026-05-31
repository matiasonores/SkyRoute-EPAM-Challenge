import { TestBed } from '@angular/core/testing';

import { ApplicationStartupService } from './application-startup.service';

describe('ApplicationStartupService', () => {
  let service: ApplicationStartupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApplicationStartupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
