import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightSearchFormComponent } from './flight-search-form.component';

describe('FlightSearchFormComponent', () => {
  let component: FlightSearchFormComponent;
  let fixture: ComponentFixture<FlightSearchFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlightSearchFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FlightSearchFormComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
