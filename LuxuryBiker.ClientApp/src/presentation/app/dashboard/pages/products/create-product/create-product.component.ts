import { HttpErrorResponse } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ProductModel } from '@domain/products/models/product.model';
import { TranslateModule } from '@ngx-translate/core';
import { ProductsService } from '@services/products/products.service';
import { ToastService } from '@shared/toast/toast.service';
import { catchError, EMPTY, finalize, tap } from 'rxjs';
import { LoaderComponent } from "@loader/loader.component";

@Component({
  selector: 'app-create-product',
  standalone: true,
  imports: [ReactiveFormsModule, TranslateModule, RouterLink, LoaderComponent],
  templateUrl: './create-product.component.html',
  styleUrl: './create-product.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class CreateProductComponent {
  processingRequest: boolean = false;
  showSuccessAlert: boolean = false;
  productCreatedCode: string | null = null;

  form = new FormGroup({
    name: new FormControl('', { validators: [Validators.required] }),
    reference: new FormControl('', { validators: [Validators.required] }),
    description: new FormControl(''),
    status: new FormControl(true),
  });

  constructor(
    private productsService: ProductsService,
    private toast: ToastService,
    private cdr: ChangeDetectorRef
  ) {}

  createProduct = async () => {
    this.productCreatedCode = null;
    this.showSuccessAlert = false;

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.processingRequest = true;
    this.productsService
      .create(this.form.value as ProductModel)
      .pipe(
        tap((productCode: string) => this.productCreated(productCode)),
        finalize(() => (this.processingRequest = false)),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 400) {
            this.handleBadRequest(error.error.errors);
            return EMPTY;
          } else if (error.status === 403) {
            this.handleUnauthorizedAccess();
            return EMPTY;
          }

          this.handleUnexpectedError();
          throw error;
        })
      )
      .subscribe();
  };

  productCreated(productCode: string) {
    this.productCreatedCode = productCode;
    this.showSuccessAlert = true;
    this.toast.success(`Producto registrado con el código ${productCode}.`);
    this.form.reset();
    this.cdr.markForCheck();
  }

  handleBadRequest(errors: ValidationErrors) {
    this.form.setErrors(errors);
    this.cdr.markForCheck();
    this.form.markAllAsTouched();
  }

  handleUnauthorizedAccess() {
    const message = 'No cuenta con los permisos necesarios para realizar la acción solicitada.';
    this.form.setErrors({ errorUnexpected: true, error: message });
    this.toast.error(message);
    this.cdr.markForCheck();
  }

  handleUnexpectedError() {
    const message = 'Ocurrio un error inesperado, por favor intente de nuevo más tarde';
    this.form.setErrors({ errorUnexpected: true, error: message });
    this.toast.error(message);
    this.cdr.markForCheck();
  }
}
