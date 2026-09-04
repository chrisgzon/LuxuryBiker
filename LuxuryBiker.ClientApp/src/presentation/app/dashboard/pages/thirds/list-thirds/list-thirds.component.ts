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
import { ThirdModel } from '@domain/thirds/models/third.model';
import { PaginatedResult } from '@domain/common/models/paginated-result.model';
import { ThirdsService } from '@services/thirds/thirds.service';
import { LoaderComponent } from '@loader/loader.component';

@Component({
  selector: 'app-list-thirds',
  standalone: true,
  imports: [CommonModule, TranslateModule, RouterLink, LoaderComponent],
  templateUrl: './list-thirds.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ListThirdsComponent implements OnInit {
  loading = false;
  loadError = false;
  page: PaginatedResult<ThirdModel> | null = null;

  readonly pageSize = 20;
  private pageNumber = 1;
  typeFilter: number | null = null;

  constructor(
    private thirdsService: ThirdsService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load(1);
  }

  onFilterChange(value: string): void {
    this.typeFilter = value ? Number(value) : null;
    this.load(1);
  }

  load(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.loading = true;
    this.loadError = false;

    this.thirdsService
      .getAll({
        pageNumber,
        pageSize: this.pageSize,
        typeId: this.typeFilter ?? undefined,
      })
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
