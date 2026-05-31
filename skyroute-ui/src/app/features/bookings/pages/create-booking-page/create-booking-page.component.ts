import { Component, computed, inject } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';

import { SelectedFlightStateService } from '../../../../core/state/selected-flight-state.service';
import { SearchCriteriaStateService } from '../../../../core/state/search-criteria-state.service';
import { BookingApiService } from '../../../../core/services/booking-api.service';
import { PassengerFormComponent } from '../../components/passenger-form/passenger-form.component';
import { Passenger } from '../../../../models/passenger.model';

@Component({
  selector: 'app-create-booking-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatDividerModule,
    MatIconModule,
    PassengerFormComponent,
  ],
  templateUrl: './create-booking-page.component.html',
  styleUrl: './create-booking-page.component.scss'
})
export class CreateBookingPageComponent {

  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly bookingApi = inject(BookingApiService);
  readonly selectedFlightState = inject(SelectedFlightStateService);
  readonly searchState = inject(SearchCriteriaStateService);

  readonly flight = computed(() => this.selectedFlightState.getFlightOrThrow());

  readonly passengerCount = computed(() =>
    this.searchState.searchCriteria()?.passengers ?? 1
  );

  readonly pricePerPassenger = computed(() => this.flight().price);

  readonly dynamicTotalPrice = computed(() =>
    this.flight().price * this.passengerCount()
  );

  readonly passengers = this.fb.array<
    FormGroup<{
      fullName: FormControl<string | null>;
      emailAddress: FormControl<string | null>;
      nationalId: FormControl<string | null>;
      passportNumber: FormControl<string | null>;
    }>
  >([]);

  readonly form = this.fb.group({
    passengers: this.passengers
  });

  constructor() {
    for (let i = 0; i < this.passengerCount(); i++) {
      this.passengers.push(this.createPassenger());
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.bookingApi.createBooking({
      flight: this.flight(),
      price: this.flight().totalPrice,
      passengers: this.passengers.getRawValue() as unknown as Passenger[]
    }).subscribe({
      next: booking => {
        this.router.navigate(['/bookings', booking.bookingReference]);
      }
    });
  }

  formatDuration(minutes: number): string {
    if (!minutes) return '—';
    const h = Math.floor(minutes / 60);
    const m = minutes % 60;
    return h > 0 ? `${h}h ${m}m` : `${m}m`;
  }

  private createPassenger() {
    return this.fb.group({
      fullName: ['', Validators.required],
      emailAddress: ['', [Validators.required, Validators.email]],
      nationalId: [''],
      passportNumber: [''],
    });
  }
}
