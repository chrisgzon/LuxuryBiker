import { ComponentFixture, TestBed } from '@angular/core/testing';
import { importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { TranslateModule } from '@ngx-translate/core';
import { provideEnvironmentNgxCurrency } from 'ngx-currency';
import {
  createPurchaseUseCaseProvider,
  getPurchasesUseCaseProvider,
  getPurchaseFormDataUseCaseProvider,
  changePurchaseStatusUseCaseProvider,
  purchasesImplementationRepositoryProvider,
} from '@data/purchases';

import RegisterPurchaseComponent from './register-purchase.component';

describe('RegisterPurchaseComponent', () => {
  let component: RegisterPurchaseComponent;
  let fixture: ComponentFixture<RegisterPurchaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterPurchaseComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting(),
        importProvidersFrom(TranslateModule.forRoot()),
        provideEnvironmentNgxCurrency({ prefix: '$ ', thousands: '.', decimal: ',', precision: 0, allowNegative: false }),
        createPurchaseUseCaseProvider,
        getPurchasesUseCaseProvider,
        getPurchaseFormDataUseCaseProvider,
        changePurchaseStatusUseCaseProvider,
        purchasesImplementationRepositoryProvider,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterPurchaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('computes subtotal, IVA and total from the added lines', () => {
    component.lines = [
      { productId: 1, productName: 'A', productCode: 'A1', productValue: 1000, quantity: 2, subtotal: 2000 },
      { productId: 2, productName: 'B', productCode: 'B1', productValue: 500, quantity: 1, subtotal: 500 },
    ];
    component.header.controls.applyIva.setValue(true);

    expect(component.subtotal).toBe(2500);
    expect(component.ivaAmount).toBeCloseTo(475, 5);
    expect(component.total).toBeCloseTo(2975, 5);
  });
});
