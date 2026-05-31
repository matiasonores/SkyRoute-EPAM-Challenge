import {
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';

import { CabinClass } from '../../../../models/enums/cabin-class.enum';
import { Airport } from '../../../../models/airport.model';
import { SearchCriteriaStateService } from '../../../../core/state/search-criteria-state.service';
import { ReferenceDataStateService } from '../../../../core/state/reference-data-state.service';
import { FlightSearchRequest } from '../../../../models/flight-search-request.model';
import { FlightApiService } from '../../../../core/services/flight-api.service';
import { FlightResultsStateService } from '../../../../core/state/flight-results-state.service';
import { airportSelectionValidator } from '../../../../core/validators/airport-selection.validator';

@Component({
  selector: 'app-flight-search-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSlideToggleModule,
    MatDividerModule,
    MatIconModule,
    NgxMatSelectSearchModule,
  ],
  templateUrl: './flight-search-form.component.html',
  styleUrl: './flight-search-form.component.scss'
})
export class FlightSearchFormComponent {
  private readonly flightApi = inject(FlightApiService);
  private readonly flightResults = inject(FlightResultsStateService);
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly searchState = inject(SearchCriteriaStateService);
  readonly referenceState = inject(ReferenceDataStateService);

  readonly airports = computed(() => this.referenceState.airports());
  readonly originSearch = signal('');
  readonly destinationSearch = signal('');
  readonly searching = signal(false);

  readonly filteredOriginAirports = computed(() => {
    const s = this.originSearch().toLowerCase().trim();
    if (!s) return this.airports();
    return this.airports().filter(a =>
      a.code.toLowerCase().includes(s) ||
      a.name.toLowerCase().includes(s) ||
      a.city.toLowerCase().includes(s)
    );
  });

  readonly filteredDestinationAirports = computed(() => {
    const s = this.destinationSearch().toLowerCase().trim();
    if (!s) return this.airports();
    return this.airports().filter(a =>
      a.code.toLowerCase().includes(s) ||
      a.name.toLowerCase().includes(s) ||
      a.city.toLowerCase().includes(s)
    );
  });

  readonly cabinClasses = [
    { value: CabinClass.Economy, label: 'Economy' },
    { value: CabinClass.Business, label: 'Business' },
    { value: CabinClass.First, label: 'First Class' },
  ];

  readonly today = new Date();

  readonly form = this.fb.group({
    origin: [null as Airport | null, Validators.required],
    destination: [null as Airport | null, Validators.required],
    departureDate: ['', Validators.required],
    passengers: [1, [Validators.required, Validators.min(1), Validators.max(9)]],
    cabinClass: [CabinClass.Economy, Validators.required],
  }, {
    validators: [airportSelectionValidator()]
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const departureDate = this.formatDate(this.form.value.departureDate!);

    const request: FlightSearchRequest = {
      passengers: this.form.value.passengers!,
      cabinClass: CabinClass[this.form.value.cabinClass!] as string,
      flightType: 'OneWay',
      flexDates: false,
      legs: [{
        origin: this.form.value.origin!.code,
        destination: this.form.value.destination!.code,
        departureDate,
      }]
    };

    this.searchState.setSearchCriteria(request);
    this.searching.set(true);
    this.flightResults.setLoading(true);

    this.flightApi.searchFlights(request).subscribe({
      next: flights => {
        this.flightResults.setFlights(flights);
        this.flightResults.setLoading(false);
        this.searching.set(false);
        this.router.navigate(['/flights/results']);
      },
      error: err => {
        console.error(err);
        this.flightResults.setLoading(false);
        this.searching.set(false);
      }
    });
  }

  increasePassengers(): void {
    const v = this.form.controls.passengers.value!;
    if (v < 9) this.form.controls.passengers.setValue(v + 1);
  }

  decreasePassengers(): void {
    const v = this.form.controls.passengers.value!;
    if (v > 1) this.form.controls.passengers.setValue(v - 1);
  }

  private formatDate(date: string | Date): string {
    return new Date(date).toISOString().slice(0, 10);
  }
}
