import {
    AbstractControl,
    ValidationErrors,
    ValidatorFn
} from '@angular/forms';

import { Airport } from  '../../models/airport.model';

export function airportSelectionValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {

        const origin = control.get('origin')?.value as Airport;

        const destination = control.get('destination')?.value as Airport;

        if (!origin || !destination) {
            return null;
        }

        if (origin.code === destination.code) {
            return {
                sameAirport: true
            };
        }

        return null;
    };
}