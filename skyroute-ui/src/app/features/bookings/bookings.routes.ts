import { Routes } from '@angular/router';

export const BOOKINGS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/booking-list-page/booking-list-page.component')
        .then(c => c.BookingListPageComponent)
  },
  {
    path: 'create',
    loadComponent: () =>
      import('./pages/create-booking-page/create-booking-page.component')
        .then(c => c.CreateBookingPageComponent)
  },
  {
    path: ':bookingReference',
    loadComponent: () =>
      import('./pages/booking-detail-page/booking-detail-page.component')
        .then(c => c.BookingDetailPageComponent)
  }
];