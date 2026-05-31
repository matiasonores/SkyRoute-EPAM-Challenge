import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Booking } from '../../models/booking.model';
import { CreateBookingRequest } from '../../models/create-booking-request.model';
import { API_CONFIG } from '../config/api.config';

@Injectable({
  providedIn: 'root'
})
export class BookingApiService {
  private readonly http = inject(HttpClient);

  createBooking(request: CreateBookingRequest): Observable<Booking> {
    return this.http.post<Booking>(`${API_CONFIG.baseUrl}/api/flights/bookings`, request);
  }

  getBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${API_CONFIG.baseUrl}/api/flights/bookings`);
  }

  getBooking(reference: string): Observable<Booking> {
    return this.http.get<Booking>(`${API_CONFIG.baseUrl}/api/flights/bookings/${reference}`);
  }
}