import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { Flight } from '../../../../models/flight.model';

@Component({
  selector: 'app-flight-card',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatDividerModule,
    MatIconModule,
  ],
  templateUrl: './flight-card.component.html',
  styleUrl: './flight-card.component.scss'
})
export class FlightCardComponent {

  @Input({ required: true }) flight!: Flight;
  @Output() book = new EventEmitter<Flight>();

  onBook(): void {
    this.book.emit(this.flight);
  }

  hasOffer(flight: Flight): boolean {
    return flight.provider === 'BudgetWings' && flight.totalPrice > 29.99;
  }

  getOriginalPrice(flight: Flight): number {
    return Number((flight.totalPrice / 0.9).toFixed(2));
  }

  formatDuration(minutes: number): string {
    if (!minutes) return '—';
    const h = Math.floor(minutes / 60);
    const m = minutes % 60;
    return h > 0 ? `${h}h ${m}m` : `${m}m`;
  }
}
