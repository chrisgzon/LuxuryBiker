import { CommonModule } from '@angular/common';
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
import { HttpErrorResponse } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NgxCurrencyDirective } from 'ngx-currency';
import { catchError, EMPTY, finalize, tap } from 'rxjs';
import {
  CreateSaleModel,
  IVA_RATE,
  SaleFormDataModel,
  SaleLineModel,
  SaleProductOption,
  SaleClientOption,
} from '@domain/sales/models/sale.model';
import { SalesService } from '@services/sales/sales.service';
import { ToastService } from '@shared/toast/toast.service';
import { LoaderComponent } from '@loader/loader.component';

@Component({
  selector: 'app-register-sale',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    RouterLink,
    LoaderComponent,
    NgxCurrencyDirective,
  ],
  templateUrl: './register-sale.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class RegisterSaleComponent implements OnInit {
  readonly ivaRate = IVA_RATE;

  loadingFormData = false;
  processingRequest = false;
  showSuccessAlert = false;
  createdCode: string | null = null;
  generalError: string | null = null;

  products: SaleProductOption[] = [];
  clients: SaleClientOption[] = [];
  lines: SaleLineModel[] = [];

  header = new FormGroup({
    clientId: new FormControl(0, { nonNullable: true }),
    applyIva: new FormControl(false, { nonNullable: true }),
  });

  lineForm = new FormGroup({
    productId: new FormControl(0, {
      validators: [Validators.required, Validators.min(1)],
      nonNullable: true,
    }),
    productValue: new FormControl(0, {
      validators: [Validators.required, Validators.min(1)],
      nonNullable: true,
    }),
    quantity: new FormControl(1, {
      validators: [Validators.required, Validators.min(1), Validators.pattern(/^[0-9]+$/)],
      nonNullable: true,
    }),
  });

  constructor(
    private salesService: SalesService,
    private toast: ToastService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadFormData();
  }

  loadFormData(): void {
    this.loadingFormData = true;
    this.generalError = null;

    this.salesService
      .getFormData()
      .pipe(
        finalize(() => {
          this.loadingFormData = false;
          this.cdr.markForCheck();
        }),
        catchError(() => {
          this.generalError = 'No se pudieron cargar los productos y clientes.';
          return EMPTY;
        })
      )
      .subscribe((data: SaleFormDataModel) => {
        this.products = data.products;
        this.clients = data.clients;
        this.cdr.markForCheck();
      });
  }

  get subtotal(): number {
    return this.lines.reduce((acc, line) => acc + line.subtotal, 0);
  }

  get ivaAmount(): number {
    return this.header.controls.applyIva.value ? this.subtotal * (this.ivaRate / 100) : 0;
  }

  get total(): number {
    return this.subtotal + this.ivaAmount;
  }

  addLine(): void {
    if (this.lineForm.invalid) {
      this.lineForm.markAllAsTouched();
      return;
    }

    const { productId, productValue, quantity } = this.lineForm.getRawValue();
    const product = this.products.find((p) => p.id === productId);
    if (!product) {
      return;
    }

    if (this.lines.some((l) => l.productId === productId)) {
      this.generalError =
        'El producto ya está en la lista. Elimínelo y agréguelo de nuevo para cambiar sus valores.';
      return;
    }

    this.generalError = null;
    this.lines = [
      ...this.lines,
      {
        productId,
        productName: product.name,
        productCode: product.code,
        productValue,
        quantity,
        subtotal: productValue * quantity,
      },
    ];

    this.lineForm.reset({ productId: 0, productValue: 0, quantity: 1 });
  }

  removeLine(index: number): void {
    this.lines = this.lines.filter((_, i) => i !== index);
  }

  registerSale(): void {
    this.showSuccessAlert = false;
    this.createdCode = null;
    this.generalError = null;

    if (this.lines.length === 0) {
      this.generalError = 'Agregue al menos un producto a la venta.';
      return;
    }

    const raw = this.header.getRawValue();
    const payload: CreateSaleModel = {
      thirdId: raw.clientId > 0 ? raw.clientId : null,
      applyIva: raw.applyIva,
      details: this.lines.map((l) => ({
        productId: l.productId,
        productValue: l.productValue,
        quantity: l.quantity,
      })),
    };

    this.processingRequest = true;
    this.salesService
      .create(payload)
      .pipe(
        tap((created) => this.onSaleCreated(created.code)),
        finalize(() => {
          this.processingRequest = false;
          this.cdr.markForCheck();
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 400) {
            this.generalError = this.firstValidationMessage(error.error?.errors);
          } else if (error.status === 403) {
            this.generalError = 'No cuenta con los permisos necesarios para registrar ventas.';
          } else {
            this.generalError =
              'Ocurrió un error inesperado, por favor intente de nuevo más tarde.';
          }
          this.toast.error(this.generalError);
          return EMPTY;
        })
      )
      .subscribe();
  }

  private onSaleCreated(code: string): void {
    this.createdCode = code;
    this.showSuccessAlert = true;
    this.toast.success(`Venta registrada con el código ${code}.`);
    this.lines = [];
    this.header.reset({ clientId: 0, applyIva: false });
    this.lineForm.reset({ productId: 0, productValue: 0, quantity: 1 });
    this.cdr.markForCheck();
  }

  private firstValidationMessage(errors: ValidationErrors | undefined): string {
    if (!errors) {
      return 'Revise los datos de la venta.';
    }
    const firstKey = Object.keys(errors)[0];
    const value = errors[firstKey];
    return Array.isArray(value) ? value[0] : String(value);
  }
}
