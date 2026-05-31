import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PassengerFormComponent } from './passenger-form.component';

describe('PassengerFormComponent', () => {
  let component: PassengerFormComponent;
  let fixture: ComponentFixture<PassengerFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PassengerFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PassengerFormComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
