import { Injectable, inject } from '@angular/core';

import { firstValueFrom } from 'rxjs';

import { ReferenceDataService }
from './reference-data.service';

import { ReferenceDataStateService }
from '../state/reference-data-state.service';

@Injectable({
  providedIn: 'root'
})
export class ApplicationStartupService {

  private readonly referenceService =
    inject(ReferenceDataService);

  private readonly referenceState =
    inject(ReferenceDataStateService);

  async initialize(): Promise<void> {

    if (
      this.referenceState.airportCount() > 0
    ) {
      return;
    }

    try {
      const response =
        await firstValueFrom(
          this.referenceService.getReferenceData()
        );

      this.referenceState.setCountries(
        response.countries
      );

      this.referenceState.setAirports(
        response.airports
      );
    } catch (error) {
      console.warn(
        'ApplicationStartupService: could not load reference data during initialization.',
        error
      );
    }
  }
}