import { Injectable, inject } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { FlightReferenceDataResponse } from '../../models/flight-reference-data-response.model';

import { API_CONFIG }
from '../config/api.config';

@Injectable({
  providedIn: 'root'
})
export class ReferenceDataService {

  private readonly http =
    inject(HttpClient);

  getReferenceData():
    Observable<FlightReferenceDataResponse> {

    return this.http.get<FlightReferenceDataResponse>(
      `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.referenceData}`
    );
  }
}