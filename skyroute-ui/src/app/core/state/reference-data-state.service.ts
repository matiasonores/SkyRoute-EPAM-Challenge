import {
  Inject,
  Injectable,
  PLATFORM_ID,
  computed,
  effect,
  signal
} from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

import { Airport } from '../../models/airport.model';
import { Country } from '../../models/country.model';

@Injectable({
  providedIn: 'root'
})
export class ReferenceDataStateService {

  private readonly countriesSignal =
    signal<Country[]>([]);

  private readonly airportsSignal =
    signal<Airport[]>([]);

  readonly countries =
    computed(() => this.countriesSignal());

  readonly airports =
    computed(() => this.airportsSignal());

  readonly airportCount =
    computed(() => this.airportsSignal().length);

  readonly countryCount =
    computed(() => this.countriesSignal().length);

  private readonly isBrowser: boolean;

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(this.platformId);

    if (!this.isBrowser) {
      return;
    }

    this.restore();

    effect(() => {
      sessionStorage.setItem(
        'reference-countries',
        JSON.stringify(this.countriesSignal())
      );

      sessionStorage.setItem(
        'reference-airports',
        JSON.stringify(this.airportsSignal())
      );
    });
  }

  setCountries(countries: Country[]): void {

    this.countriesSignal.set(countries);
  }

  setAirports(airports: Airport[]): void {

    this.airportsSignal.set(airports);
  }

  clear(): void {

    this.countriesSignal.set([]);

    this.airportsSignal.set([]);
  }

  private restore(): void {
    if (!this.isBrowser) {
      return;
    }

    const countries =
      sessionStorage.getItem('reference-countries');

    const airports =
      sessionStorage.getItem('reference-airports');

    if (countries) {

      this.countriesSignal.set(
        JSON.parse(countries)
      );
    }

    if (airports) {

      this.airportsSignal.set(
        JSON.parse(airports)
      );
    }
  }
}