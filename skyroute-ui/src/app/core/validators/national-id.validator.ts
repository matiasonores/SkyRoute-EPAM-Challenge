import {
    AbstractControl,
    ValidationErrors,
    ValidatorFn
} from '@angular/forms';

export function nationalIdValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value?.trim();
        if (!value) {
            return null;
        }
        const regex = /^\d{8}$/;
        return regex.test(value) ? null : {
            invalidNationalId: true
        };
    };
}