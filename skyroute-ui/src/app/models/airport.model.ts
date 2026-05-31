import { Country } from './country.model';

export interface Airport {
  id: number;
  code: string;
  name: string;
  city: string;
  countryId: number;
  country: Country;
}