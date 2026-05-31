import { Airport } from './airport.model';
import { CabinClass } from './enums/cabin-class.enum';
import { FlightStatus } from './enums/flight-status.enum';

export interface Flight {
    id: string;
    airline: string;
    provider: string;
    flightNumber: string;
    origin: Airport;
    destination: Airport;
    departure: string;
    arrival: string;
    duration: string;
    durationMinutes: number;
    cabinClass: CabinClass;
    passengers: number;
    price: number;
    totalPrice: number;
    isInternational: boolean;
    status?: FlightStatus;
}