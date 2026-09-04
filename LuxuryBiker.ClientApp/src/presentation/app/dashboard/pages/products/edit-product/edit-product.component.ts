import { HttpErrorResponse } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, EMPTY, finalize, tap } from 'rxjs';
import { ProductModel } from '@domain/products/models/product.model';
import { ProductsService } from '@services/products/products.service';
import { ToastService } from '@shared/toast/toast.service';
import { LoaderComponent } from '@loader/loader.component';

@Component({
  selector: 'app-edit-product',
  standalone: true,
  imports: [ReactiveFormsModule, TranslateModule, RouterLink, LoaderComponent],
  templateUrl: './edit-product.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class EditProductComponent implements OnInit {
  loading = false;
  processingRequest = false;
  loadError = false;
  private productId = 0;

  form = new FormGroup({
    name: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    reference: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    description: new FormControl<string | null>(null),
    status: new FormControl(true, { nonNullable: true }),
  });

  constructor(
    private productsService: ProductsService,
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.productId = Number(this.route.snapshot.paramMap.get('id'));
    this.loading = true;

    this.productsService
      .getById(this.productId)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.cdr.markForCheck();
        }),
        catchError(() => {
          this.loadError = true;
          return EMPTY;
        })
      )
      .subscribe((product) => {
        this.form.patchValue({
          name: product.name,
          reference: product.reference,
          description: product.description ?? null,
          status: product.status,
        });
        this.cdr.markForCheck();
      });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();
    const payload = {
      id: this.productId,
      name: raw.name,
      reference: raw.reference,
      description: raw.description ?? '',
      status: raw.status,
    } as ProductModel;

    this.processingRequest = true;
    this.productsService
      .update(payload)
      .pipe(
        tap(() => {
          this.toast.success('Producto actualizado correctamente.');
          this.router.navigate(['/products/list']);
        }),
        finalize(() => {
          this.processingRequest = false;
          this.cdr.markForCheck();
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 400) {
            this.handleBadRequest(error.error?.errors);
          } else if (error.status === 403) {
            this.setFormError('No cuenta con los permisos necesarios para editar productos.');
          } else if (error.status === 404) {
            this.setFormError('El producto ya no existe.');
          } else {
            this.setFormError('Ocurrió un error inesperado, intente de nuevo más tarde.');
          }
          return EMPTY;
        })
      )
      .subscribe();
  }

  private handleBadRequest(errors: ValidationErrors | undefined): void {
    this.form.setErrors(errors ?? { errorUnexpected: true });
    this.form.markAllAsTouched();
    this.cdr.markForCheck();
  }

  private setFormError(message: string): void {
    this.form.setErrors({ errorUnexpected: true, error: message });
    this.toast.error(message);
    this.cdr.markForCheck();
  }
}
