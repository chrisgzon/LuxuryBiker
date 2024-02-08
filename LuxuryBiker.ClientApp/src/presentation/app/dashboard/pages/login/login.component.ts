import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginCredentialsModel } from '@models/auth/login-credentials.model';
import { AuthService } from '@services/auth/auth.service';
import { EMPTY, catchError, finalize } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ ReactiveFormsModule ],
  templateUrl: './login.component.html',
  styles: ``,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class LoginComponent {
  processingRequest = false;

  form = new FormGroup({
    username: new FormControl('', {
      validators: [Validators.required],
      nonNullable: true,
    }),
    password: new FormControl('', {
      validators: [Validators.required],
      nonNullable: true,
    }),
  });

  constructor(
    public authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  login() {
    this.processingRequest = true;

    this.authService
      .login(this.form.value as LoginCredentialsModel)
      .pipe(
        finalize(() => (this.processingRequest = false)),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            this.handleUnauthorized();
            return EMPTY;
          }

          throw error;
        })
      )
      .subscribe();
  }

  handleUnauthorized() {
    this.form.setErrors({ invalidCredentials: true });
    this.cdr.markForCheck();
  }
}
