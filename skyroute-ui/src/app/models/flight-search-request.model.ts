import { FlightSearchLeg } from './flight-search-leg.model';

export interface FlightSearchRequest {
  passengers: number;

  cabinClass: string;

  flightType: string;

  flexDates: boolean;

  legs: FlightSearchLeg[];
}