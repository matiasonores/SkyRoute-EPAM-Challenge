import { Routes } from '@angular/router';

import { FlightResultsPageComponent } from './pages/flight-results-page/flight-results-page.component';

export const FLIGHTS_ROUTES: Routes = [

  {
    path: 'results',
    component: FlightResultsPageComponent
  }
];