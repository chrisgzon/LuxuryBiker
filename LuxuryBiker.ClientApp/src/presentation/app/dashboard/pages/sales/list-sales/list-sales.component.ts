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
import { SaleListItemModel } from '@domain/sales/models/sale.model';
import { SalesService } from '@services/sales/sales.service';
import { ToastService } from '@shared/toast/toast.service';
import { LoaderComponent } from '@loader/loader.component';

@Component({
  selector: 'app-list-sales',
  standalone: true,
  imports: [CommonModule, TranslateModule, RouterLink, LoaderComponent],
  templateUrl: './list-sales.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ListSalesComponent implements OnInit {
  loading = false;
  loadError = false;
  actionError: string | null = null;
  togglingId: number | null = null;
  page: PaginatedResult<SaleListItemModel> | null = null;

  readonly pageSize = 20;
  private pageNumber = 1;

  constructor(
    private salesService: SalesService,
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

    this.salesService
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

  toggleStatus(sale: SaleListItemModel): void {
    const key = sale.status
      ? 'sales.list.confirmCancel'
      : 'sales.list.confirmValidate';
    if (!window.confirm(this.translate.instant(key))) {
      return;
    }

    this.actionError = null;
    this.togglingId = sale.id;

    this.salesService
      .changeStatus(sale.id)
      .pipe(
        finalize(() => {
          this.togglingId = null;
          this.cdr.markForCheck();
        }),
        catchError((error) => {
          this.actionError =
            error?.status === 403
              ? 'No cuenta con los permisos necesarios para esta acción.'
              : 'No se pudo actualizar el estado de la venta.';
          this.toast.error(this.actionError);
          return EMPTY;
        })
      )
      .subscribe((result) => {
        sale.status = result.status;
        this.toast.success(
          result.status ? 'Venta validada. Stock actualizado.' : 'Venta cancelada. Stock reintegrado.'
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
