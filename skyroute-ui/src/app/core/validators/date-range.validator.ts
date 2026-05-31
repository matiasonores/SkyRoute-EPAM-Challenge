import {
    AbstractControl,
    ValidationErrors,
    ValidatorFn
} from '@angular/forms';

export function dateRangeValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const departure = control.get('departureDate')?.value;
        const returnDate = control.get('returnDate')?.value;

        if (!departure || !returnDate) {
            return null;
        }
        if (departure > returnDate) {
            return {
                invalidDateRange: true
            };
        }
        return null;
    };
}