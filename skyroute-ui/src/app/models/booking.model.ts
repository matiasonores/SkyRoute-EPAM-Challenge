import { Flight } from './flight.model';
import { Passenger } from './passenger.model';

export interface Booking {
  id: string;
  bookingReference: string;
  flightNumber: string;
  price: number;
  totalPrice: number;
  createdAt: string;
  status: string;
  passengerCount: number;
  flight?: Flight;
  passengers?: Passenger[];
}