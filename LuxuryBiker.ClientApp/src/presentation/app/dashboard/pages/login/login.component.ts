import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { LoginCredentialsModel } from '@domain/authentication/models/login-credentials.model';
import { AuthService } from '@services/auth/auth.service';
import { EMPTY, catchError, finalize } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ ReactiveFormsModule ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class LoginComponent {
  processingRequest = false;

  form = new FormGroup({
    username: new FormControl('', {
      validators: [
        Validators.required, Validators.email
      ],
      nonNullable: true,
    }),
    password: new FormControl('', {
      validators: [Validators.required],
      nonNullable: true,
    }),
    rememberme: new FormControl(false, {
      nonNullable: true,
    })
  });

  constructor(
    public authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  login() {

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.processingRequest = true;

    this.authService
      .login(this.form.value as LoginCredentialsModel)
      .pipe(
        finalize(() => (this.processingRequest = false)),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 400) {
            this.handleUnauthorized(error.error.errors);
            return EMPTY;
          }

          this.handleUnexpectedError();
          throw error;
        })
      )
      .subscribe();
  }

  handleUnauthorized(errors: ValidationErrors) {
    this.form.setErrors(errors);
    this.cdr.markForCheck();
  }

  handleUnexpectedError() {
    this.form.setErrors({
      errorUnexpected: true,
      error: "Ocurrio un error inesperado, por favor intente de nuevo más tarde" 
    });
    this.cdr.markForCheck();
  }
}
