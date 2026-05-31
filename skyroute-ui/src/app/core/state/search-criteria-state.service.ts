import {
  Inject,
  Injectable,
  PLATFORM_ID,
  computed,
  effect,
  signal
} from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

import { FlightSearchRequest }
from '../../models/flight-search-request.model';

@Injectable({
  providedIn: 'root'
})
export class SearchCriteriaStateService {

  private readonly isBrowser: boolean;

  private readonly searchSignal =
    signal<FlightSearchRequest | null>(null);

  readonly searchCriteria =
    computed(() => this.searchSignal());

  readonly hasSearchCriteria =
    computed(() => this.searchSignal() !== null);

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(this.platformId);

    if (!this.isBrowser) {
      return;
    }

    this.restore();

    effect(() => {

      const criteria =
        this.searchSignal();

      if (!criteria) {

        sessionStorage.removeItem(
          'flight-search-criteria'
        );

        return;
      }

      sessionStorage.setItem(
        'flight-search-criteria',
        JSON.stringify(criteria)
      );
    });
  }

  setSearchCriteria(
    criteria: FlightSearchRequest
  ): void {

    this.searchSignal.set(criteria);
  }

  clear(): void {

    this.searchSignal.set(null);
  }

  private restore(): void {
    if (!this.isBrowser) {
      return;
    }

    const saved =
      sessionStorage.getItem(
        'flight-search-criteria'
      );

    if (!saved) {
      return;
    }

    this.searchSignal.set(
      JSON.parse(saved)
    );
  }
}