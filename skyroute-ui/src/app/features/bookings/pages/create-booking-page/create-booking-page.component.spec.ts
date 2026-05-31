import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBookingPageComponent } from './create-booking-page.component';

describe('CreateBookingPageComponent', () => {
  let component: CreateBookingPageComponent;
  let fixture: ComponentFixture<CreateBookingPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateBookingPageComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateBookingPageComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
