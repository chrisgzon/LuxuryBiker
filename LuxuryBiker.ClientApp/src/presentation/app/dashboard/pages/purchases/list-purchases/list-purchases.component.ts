import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { catchError, EMPTY, finalize } from 'rxjs';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { PurchaseListItemModel } from '@domain/purchases/models/purchase.model';
import { PurchasesService } from '@services/purchases/purchases.service';
import { ToastService } from '@shared/toast/toast.service';
import { LoaderComponent } from '@loader/loader.component';

@Component({
  selector: 'app-list-purchases',
  standalone: true,
  imports: [CommonModule, TranslateModule, RouterLink, LoaderComponent],
  templateUrl: './list-purchases.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ListPurchasesComponent implements OnInit {
  loading = false;
  loadError = false;
  actionError: string | null = null;
  togglingId: number | null = null;
  page: PaginatedResult<PurchaseListItemModel> | null = null;

  readonly pageSize = 20;
  private pageNumber = 1;

  constructor(
    private purchasesService: PurchasesService,
    private translate: TranslateService,
    private toast: ToastService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load(1);
  }

  load(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.loading = true;
    this.loadError = false;
    this.actionError = null;

    this.purchasesService
      .getAll({ pageNumber, pageSize: this.pageSize })
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
      .subscribe((result) => {
        this.page = result;
        this.cdr.markForCheck();
      });
  }

  toggleStatus(purchase: PurchaseListItemModel): void {
    const key = purchase.status
      ? 'purchases.list.confirmCancel'
      : 'purchases.list.confirmValidate';
    if (!window.confirm(this.translate.instant(key))) {
      return;
    }

    this.actionError = null;
    this.togglingId = purchase.id;

    this.purchasesService
      .changeStatus(purchase.id)
      .pipe(
        finalize(() => {
          this.togglingId = null;
          this.cdr.markForCheck();
        }),
        catchError((error) => {
          this.actionError =
            error?.status === 403
              ? 'No cuenta con los permisos necesarios para esta acción.'
              : 'No se pudo actualizar el estado de la compra.';
          this.toast.error(this.actionError);
          return EMPTY;
        })
      )
      .subscribe((result) => {
        purchase.status = result.status;
        this.toast.success(
          result.status ? 'Compra validada. Stock actualizado.' : 'Compra cancelada. Stock revertido.'
        );
        this.cdr.markForCheck();
      });
  }

  previousPage(): void {
    if (this.page?.hasPreviousPage) {
      this.load(this.pageNumber - 1);
    }
  }

  nextPage(): void {
    if (this.page?.hasNextPage) {
      this.load(this.pageNumber + 1);
    }
  }
}
