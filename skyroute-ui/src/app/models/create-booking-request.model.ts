import { Flight } from './flight.model';
import { Passenger } from './passenger.model';

export interface CreateBookingRequest {
  flight: Flight;
  price: number;
  passengers: Passenger[];
}