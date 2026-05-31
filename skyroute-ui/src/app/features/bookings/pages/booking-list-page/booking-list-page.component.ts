import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { BookingApiService } from '../../../../core/services/booking-api.service';
import { Booking } from '../../../../models/booking.model';

@Component({
  selector: 'app-booking-list-page',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatTooltipModule,
    MatCardModule,
    MatDividerModule,
  ],
  templateUrl: './booking-list-page.component.html',
  styleUrl: './booking-list-page.component.scss'
})
export class BookingListPageComponent implements OnInit {
  private readonly bookingApi = inject(BookingApiService);
  private readonly router = inject(Router);

  readonly bookings = signal<Booking[]>([]);
  readonly loading = signal(true);

  readonly displayedColumns = [
    'bookingReference',
    'status',
    'flightNumber',
    'createdAt',
    'passengers',
    'totalPrice',
    'actions',
  ];

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.loading.set(true);
    this.bookingApi.getBookings().subscribe({
      next: bookings => {
        const orderedBookings = bookings.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        this.bookings.set(orderedBookings);
        this.loading.set(false);
      },
      error: err => {
        console.error(err);
        this.loading.set(false);
      }
    });
  }

  viewBooking(booking: Booking): void {
    this.router.navigate(['/bookings', booking.bookingReference]);
  }

  searchFlights(): void {
    this.router.navigate(['/']);
  }

  getStatusClass(status: string): string {
    return `status-${status.toLowerCase()}`;
  }
}
