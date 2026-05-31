import {
    AbstractControl,
    ValidationErrors,
    ValidatorFn
} from '@angular/forms';

export function passportNumberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value?.trim();
        if (!value) {
            return null;
        }
        const regex = /^[A-Z0-9]{9}$/i;
        return regex.test(value) ? null : {
            invalidPassportNumber: true
        };
    };
}