import { Component, computed, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FlightCardComponent } from '../../components/flight-card/flight-card.component';
import { FlightResultsStateService } from '../../../../core/state/flight-results-state.service';
import { SelectedFlightStateService } from '../../../../core/state/selected-flight-state.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { Flight } from '../../../../models/flight.model';

@Component({
  selector: 'app-flight-results-page',
  standalone: true,
  imports: [
    CommonModule,
    FlightCardComponent,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
  ],
  templateUrl: './flight-results-page.component.html',
  styleUrl: './flight-results-page.component.scss'
})
export class FlightResultsPageComponent {
  private readonly router = inject(Router);
  private readonly flightResultsState = inject(FlightResultsStateService);
  private readonly selectedFlight = inject(SelectedFlightStateService);

  readonly flights = this.flightResultsState.sortedFlights;
  readonly loading = this.flightResultsState.loading;
  readonly sortOption = this.flightResultsState.sortOption;

  readonly totalResults = computed(() => this.flights().length);

  bookFlight(flight: Flight): void {
    this.selectedFlight.setFlight(flight);
    this.router.navigate(['/bookings/create']);
  }

  changeSort(option: string): void {
    this.flightResultsState.setSort(option as any);
  }
}
