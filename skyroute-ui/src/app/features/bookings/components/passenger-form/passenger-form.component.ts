import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { nationalIdValidator } from '../../../../core/validators/national-id.validator';
import { passportNumberValidator } from '../../../../core/validators/passport-number.validator';
@Component({
  selector: 'app-passenger-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './passenger-form.component.html',
  styleUrl: './passenger-form.component.scss'
})
export class PassengerFormComponent implements OnInit {

  @Input({ required: true }) form!: FormGroup;
  @Input({ required: true }) isInternational!: boolean;
  @Input() index: number = 0;

  ngOnInit(): void {
    const passportCtrl = this.form.controls['passportNumber'];
    const nationalIdCtrl = this.form.controls['nationalId'];

    if (this.isInternational) {
      passportCtrl.setValidators([Validators.required,passportNumberValidator()]);
      nationalIdCtrl.clearValidators();
    } else {
      nationalIdCtrl.setValidators([Validators.required,nationalIdValidator()]);
      passportCtrl.clearValidators();
    }

    passportCtrl.updateValueAndValidity({ emitEvent: false });
    nationalIdCtrl.updateValueAndValidity({ emitEvent: false });
  }
}
