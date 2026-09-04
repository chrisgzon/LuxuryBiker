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
  CreatePurchaseModel,
  IVA_RATE,
  PurchaseFormDataModel,
  PurchaseLineModel,
  PurchaseProductOption,
  PurchaseSupplierOption,
} from '@domain/purchases/models/purchase.model';
import { PurchasesService } from '@services/purchases/purchases.service';
import { ToastService } from '@shared/toast/toast.service';
import { LoaderComponent } from '@loader/loader.component';

@Component({
  selector: 'app-register-purchase',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    RouterLink,
    LoaderComponent,
    NgxCurrencyDirective,
  ],
  templateUrl: './register-purchase.component.html',
  styleUrl: './register-purchase.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class RegisterPurchaseComponent implements OnInit {
  readonly ivaRate = IVA_RATE;
  readonly maxDate: string = new Date().toISOString().split('T')[0];

  loadingFormData = false;
  processingRequest = false;
  showSuccessAlert = false;
  createdCode: string | null = null;
  generalError: string | null = null;

  products: PurchaseProductOption[] = [];
  suppliers: PurchaseSupplierOption[] = [];
  lines: PurchaseLineModel[] = [];

  header = new FormGroup({
    datePurchase: new FormControl('', {
      validators: [Validators.required],
      nonNullable: true,
    }),
    supplierId: new FormControl(0, {
      validators: [Validators.required, Validators.min(1)],
      nonNullable: true,
    }),
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
    private purchasesService: PurchasesService,
    private toast: ToastService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadFormData();
  }

  loadFormData(): void {
    this.loadingFormData = true;
    this.generalError = null;

    this.purchasesService
      .getFormData()
      .pipe(
        finalize(() => {
          this.loadingFormData = false;
          this.cdr.markForCheck();
        }),
        catchError(() => {
          this.generalError = 'No se pudieron cargar los productos y proveedores.';
          return EMPTY;
        })
      )
      .subscribe((data: PurchaseFormDataModel) => {
        this.products = data.products;
        this.suppliers = data.suppliers;
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

  registerPurchase(): void {
    this.showSuccessAlert = false;
    this.createdCode = null;
    this.generalError = null;

    if (this.header.invalid) {
      this.header.markAllAsTouched();
      return;
    }
    if (this.lines.length === 0) {
      this.generalError = 'Agregue al menos un producto a la compra.';
      return;
    }

    const raw = this.header.getRawValue();
    const payload: CreatePurchaseModel = {
      thirdId: raw.supplierId,
      datePurchase: new Date(raw.datePurchase).toISOString(),
      applyIva: raw.applyIva,
      details: this.lines.map((l) => ({
        productId: l.productId,
        productValue: l.productValue,
        quantity: l.quantity,
      })),
    };

    this.processingRequest = true;
    this.purchasesService
      .create(payload)
      .pipe(
        tap((created) => this.onPurchaseCreated(created.code)),
        finalize(() => {
          this.processingRequest = false;
          this.cdr.markForCheck();
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 400) {
            this.generalError = this.firstValidationMessage(error.error?.errors);
          } else if (error.status === 403) {
            this.generalError =
              'No cuenta con los permisos necesarios para registrar compras.';
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

  private onPurchaseCreated(code: string): void {
    this.createdCode = code;
    this.showSuccessAlert = true;
    this.toast.success(`Compra registrada con el código ${code}.`);
    this.lines = [];
    this.header.reset({ datePurchase: '', supplierId: 0, applyIva: false });
    this.lineForm.reset({ productId: 0, productValue: 0, quantity: 1 });
    this.cdr.markForCheck();
  }

  private firstValidationMessage(errors: ValidationErrors | undefined): string {
    if (!errors) {
      return 'Revise los datos de la compra.';
    }
    const firstKey = Object.keys(errors)[0];
    const value = errors[firstKey];
    return Array.isArray(value) ? value[0] : String(value);
  }
}
