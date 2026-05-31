import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightResultsPageComponent } from './flight-results-page.component';

describe('FlightResultsPageComponent', () => {
  let component: FlightResultsPageComponent;
  let fixture: ComponentFixture<FlightResultsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlightResultsPageComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FlightResultsPageComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
