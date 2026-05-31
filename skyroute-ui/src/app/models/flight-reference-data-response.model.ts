import { Airport } from './airport.model';
import { Country } from './country.model';

export interface FlightReferenceDataResponse {
  countries: Country[];
  airports: Airport[];
}