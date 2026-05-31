import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BookingApiService } from '../../../../core/services/booking-api.service';
import { Booking } from '../../../../models/booking.model';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-booking-detail-page',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatDividerModule,
    MatIconModule,
  ],
  templateUrl: './booking-detail-page.component.html',
  styleUrl: './booking-detail-page.component.scss'
})
export class BookingDetailPageComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly bookingApi = inject(BookingApiService);

  readonly booking = signal<Booking | null>(null);
  readonly loading = signal(true);

  constructor() {
    const reference = this.route.snapshot.paramMap.get('bookingReference');
    if (!reference) {
      this.loading.set(false);
      return;
    }
    this.loadBooking(reference);
  }

  private loadBooking(reference: string): void {
    this.bookingApi.getBooking(reference).subscribe({
      next: booking => {
        this.booking.set(booking);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  getStatus(status: string | number | undefined): string {
    if (typeof status === 'string') return status;
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'Confirmed';
      case 2: return 'Cancelled';
      default: return 'Unknown';
    }
  }

  getStatusClass(status: string | number | undefined): string {
    return `status-${this.getStatus(status).toLowerCase()}`;
  }
}