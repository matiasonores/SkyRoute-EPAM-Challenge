import { Routes } from '@angular/router';

import { ShellComponent } from './layout/shell/shell.component';
import { NotFoundPageComponent } from './shared/components/not-found-page/not-found-page.component';

export const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      {
        path: '',
        loadChildren: () =>
          import('./features/home/home.routes')
            .then(m => m.HOME_ROUTES)
      },

      {
        path: 'flights',
        loadChildren: () =>
          import('./features/flights/flights.routes')
            .then(m => m.FLIGHTS_ROUTES)
      },

      {
        path: 'bookings',
        loadChildren: () =>
          import('./features/bookings/bookings.routes')
            .then(m => m.BOOKINGS_ROUTES)
      }
    ]
  },

  {
    path: '404',
    component: NotFoundPageComponent
  },

  {
    path: '**',
    redirectTo: '404'
  }
];