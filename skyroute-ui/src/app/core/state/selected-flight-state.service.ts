import { computed, effect, Inject, Injectable, PLATFORM_ID, signal } from "@angular/core";
import { Flight } from "../../models/flight.model";
import { isPlatformBrowser } from "@angular/common";

@Injectable({
  providedIn: 'root'
})
export class SelectedFlightStateService {

  private readonly STORAGE_KEY = 'selected-flight';

  private readonly isBrowser: boolean;

  private readonly selectedFlightSignal = signal<Flight | null>(null);

  readonly flight = computed(() =>
    this.selectedFlightSignal()
  );

  readonly hasFlight = computed(() =>
    this.selectedFlightSignal() !== null
  );

  readonly isInternational = computed(() =>
    this.selectedFlightSignal()
      ?.isInternational ?? false
  );

  readonly provider = computed(() =>
    this.selectedFlightSignal()
      ?.provider
  );

  constructor(@Inject(PLATFORM_ID) private platformId: object
  ) {

    this.isBrowser = isPlatformBrowser(this.platformId);

    if (!this.isBrowser) {
      return;
    }

    this.restoreFlight();

    effect(() => {

      const flight = this.selectedFlightSignal();

      if (!flight) {

        sessionStorage.removeItem(this.STORAGE_KEY);

        return;
      }

      sessionStorage.setItem(this.STORAGE_KEY, JSON.stringify(flight));
    });
  }

  setFlight(flight: Flight): void {
    this.selectedFlightSignal.set(
      flight
    );
  }

  clear(): void {
    this.selectedFlightSignal.set(
      null
    );
  }

  getFlightOrThrow(): Flight {
    const flight = this.selectedFlightSignal();

    if (!flight) {
      throw new Error(
        'No flight selected.'
      );
    }
    return flight;
  }

  private restoreFlight(): void {
    const saved = sessionStorage.getItem(this.STORAGE_KEY);
    if (!saved) {
      return;
    }
    this.selectedFlightSignal.set(JSON.parse(saved));
  }
}