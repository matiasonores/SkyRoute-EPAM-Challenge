import { Injectable, inject } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { Flight } from '../../models/flight.model';

import { FlightSearchRequest } from '../../models/flight-search-request.model';

import { API_CONFIG } from '../config/api.config';

@Injectable({
  providedIn: 'root'
})
export class FlightApiService {

  private readonly http =
    inject(HttpClient);

  searchFlights(
    request: FlightSearchRequest
  ): Observable<Flight[]> {

    return this.http.post<Flight[]>(
      `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.searchFlights}`,
      request
    );
  }

  getPersistedFlights():
    Observable<Flight[]> {

    return this.http.get<Flight[]>(
      `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.persistedFlights}`
    );
  }

  getFlight(
    flightNumber: string
  ): Observable<Flight> {

    return this.http.get<Flight>(
      `${API_CONFIG.baseUrl}/api/flights/${flightNumber}`
    );
  }
}