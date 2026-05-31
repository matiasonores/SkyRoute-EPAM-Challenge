import { Injectable, computed, signal } from '@angular/core';
import { Flight } from '../../models/flight.model';
import { FlightSortOption } from '../../models/enums/flight-sort-option.enum';
@Injectable({
  providedIn: 'root'
})
export class FlightResultsStateService {
  private readonly STORAGE_KEY = 'skyroute_flight_results';
  readonly flights = signal<Flight[]>([]);
  readonly loading = signal(false);
  readonly sortOption = signal<FlightSortOption>(FlightSortOption.DepartureAscending);
  readonly count = computed(() =>
    this.flights().length
  );
  readonly hasResults = computed(() =>
    this.flights().length > 0
  );
  readonly isEmpty = computed(() =>
    !this.loading() &&
    this.flights().length === 0
  );
  readonly sortedFlights = computed(() => {
    const flights = [...this.flights()];

    switch (this.sortOption()) {
      case FlightSortOption.PriceAscending:
        return flights.sort((a, b) => a.totalPrice - b.totalPrice);
      case FlightSortOption.PriceDescending:
        return flights.sort((a, b) => b.totalPrice - a.totalPrice);
      case FlightSortOption.DurationAscending:
        return flights.sort((a, b) => this.getDurationInMinutes(a) - this.getDurationInMinutes(b));
      case FlightSortOption.DepartureAscending:
        return flights.sort((a, b) => new Date(a.departure).getTime() - new Date(b.departure).getTime());
      default:
        return flights;
    }
  });
  constructor() {
    this.restoreFlights();
  }
  setFlights(flights: Flight[]): void {
    this.flights.set(flights);
    if (typeof sessionStorage !== 'undefined') {
      sessionStorage.setItem(this.STORAGE_KEY, JSON.stringify(flights));
    }
  }
  setLoading(value: boolean): void {
    this.loading.set(value);
  }
  setSort(
    option: FlightSortOption
  ): void {
    this.sortOption.set(option);
  }
  clear(): void {
    this.flights.set([]);
    if (typeof sessionStorage !== 'undefined') {
      sessionStorage.removeItem(
        this.STORAGE_KEY
      );
    }
  }
  private restoreFlights(): void {
    if (typeof sessionStorage === 'undefined') {
      return;
    }

    const stored =
      sessionStorage.getItem(
        this.STORAGE_KEY
      );
    if (!stored) {
      return;
    }
    this.flights.set(JSON.parse(stored));
  }
  private getDurationInMinutes(flight: Flight): number {
    const departure = new Date(flight.departure).getTime();
    const arrival = new Date(flight.arrival).getTime();
    return (arrival - departure) / 60000;
  }
}