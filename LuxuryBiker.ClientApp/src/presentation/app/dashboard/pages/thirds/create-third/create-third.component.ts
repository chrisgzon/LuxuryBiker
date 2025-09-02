import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ThirdModel } from '@domain/thirds/models/third.model';
import { TranslateModule } from '@ngx-translate/core';
import { ThirdsService } from '@services/thirds/thirds.service';
import { catchError, EMPTY, finalize, tap } from 'rxjs';

@Component({
  selector: 'app-create-third',
  standalone: true,
  imports: [TranslateModule, RouterLink, ReactiveFormsModule],
  templateUrl: './create-third.component.html',
  styleUrl: './create-third.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class CreateThirdComponent {
  processingRequest: boolean = false;
  showSuccessAlert: boolean = false;
  thirdCreatedId: number | null = null;

  form = new FormGroup({
    typeId: new FormControl(0, {
      validators: [Validators.required, Validators.min(1)],
    }),
    identification: new FormControl('', {
      validators: [Validators.required],
    }),
    name: new FormControl('', {
      validators: [Validators.required],
    }),
    surnames: new FormControl('', {}),
    cellPhone: new FormControl('', {
      validators: [Validators.required],
    }),
    email: new FormControl('', {
      validators: [Validators.email],
    }),
    address: new FormControl('', {
      validators: [Validators.required],
    }),
  });

  constructor(
    private thirdsService: ThirdsService,
    private cdr: ChangeDetectorRef
  ) {}

  createThird = async () => {
    this.thirdCreatedId = null;
    this.showSuccessAlert = false;
    
        console.log('Creating third', this.form.value);
    if (this.form.invalid) {
          console.log('invalid');
      this.form.markAllAsTouched();
      return;
    }

    this.processingRequest = true;
    this.thirdsService
    .create(this.form.value as ThirdModel)
    .pipe(
      tap((thirdId: number) => this.thirdCreated(thirdId)),
      finalize(() => (this.processingRequest = false)),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          this.handleBadRequest(error.error.errors);
          return EMPTY;
        }

        this.handleUnexpectedError();
        throw error;
      })
    )
    .subscribe();
  };

  thirdCreated(thirdId: number) {
    this.thirdCreatedId = thirdId;
    this.showSuccessAlert = true;
    this.form.reset();
    this.cdr.markForCheck();
  }

  handleBadRequest(errors: ValidationErrors) {
    this.form.setErrors(errors);
    this.cdr.markForCheck();
    this.form.markAllAsTouched();
  }

  handleUnexpectedError() {
    this.form.setErrors({
      errorUnexpected: true,
      error:
        'Ocurrio un error inesperado, por favor intente de nuevo más tarde',
    });
    this.cdr.markForCheck();
  }
}
