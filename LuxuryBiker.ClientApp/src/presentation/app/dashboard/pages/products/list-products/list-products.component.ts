import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, EMPTY, finalize } from 'rxjs';
import { ProductModel } from '@domain/products/models/product.model';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { ProductsService } from '@services/products/products.service';
import { LoaderComponent } from '@loader/loader.component';

@Component({
  selector: 'app-list-products',
  standalone: true,
  imports: [CommonModule, TranslateModule, RouterLink, LoaderComponent],
  templateUrl: './list-products.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ListProductsComponent implements OnInit {
  loading = false;
  loadError = false;
  page: PaginatedResult<ProductModel> | null = null;

  readonly pageSize = 20;
  private pageNumber = 1;

  constructor(
    private productsService: ProductsService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load(1);
  }

  load(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.loading = true;
    this.loadError = false;

    this.productsService
      .getAll({ pageNumber, pageSize: this.pageSize, onlyActive: false })
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
