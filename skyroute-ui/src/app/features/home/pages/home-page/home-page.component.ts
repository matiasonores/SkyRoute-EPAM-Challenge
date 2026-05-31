import { Component } from '@angular/core';

import { FlightSearchFormComponent } from '../../components/flight-search-form/flight-search-form.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [
    FlightSearchFormComponent
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
}