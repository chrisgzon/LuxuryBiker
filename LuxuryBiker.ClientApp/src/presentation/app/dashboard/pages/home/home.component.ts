import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, EMPTY, finalize } from 'rxjs';
import { DashboardModel } from '@domain/dashboard/models/dashboard.model';
import { DashboardService } from '@services/dashboard/dashboard.service';
import { LoaderComponent } from '@loader/loader.component';

interface Rect {
  x: number;
  y: number;
  w: number;
  h: number;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, TranslateModule, LoaderComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class HomeComponent implements OnInit {
  loading = false;
  loadError = false;
  data: DashboardModel | null = null;

  // Geometría de los gráficos SVG (coordenadas de viewBox).
  readonly chartW = 760;
  readonly chartH = 260;
  readonly pad = 36;

  monthlyMax = 0;
  dailyMax = 0;
  baselineY = 0;

  purchaseBars: Rect[] = [];
  saleBars: Rect[] = [];
  monthLabels: { x: number; text: string }[] = [];
  dailyPoints: { x: number; y: number; date: string; total: number }[] = [];
  dailyLine = '';

  constructor(
    private dashboardService: DashboardService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.loadError = false;

    this.dashboardService
      .get()
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
      .subscribe((data) => {
        this.data = data;
        this.buildGeometry(data);
        this.cdr.markForCheck();
      });
  }

  private buildGeometry(data: DashboardModel): void {
    const innerW = this.chartW - this.pad * 2;
    const innerH = this.chartH - this.pad * 2;
    this.baselineY = this.pad + innerH;

    this.monthlyMax = Math.max(
      1,
      ...data.purchasesByMonth.map((m) => m.total),
      ...data.salesByMonth.map((m) => m.total)
    );
    this.dailyMax = Math.max(1, ...data.salesLast15Days.map((d) => d.total));

    const slot = innerW / 12;
    const barW = slot * 0.32;

    this.purchaseBars = data.purchasesByMonth.map((m, i) => {
      const h = (m.total / this.monthlyMax) * innerH;
      return { x: this.pad + i * slot + slot / 2 - barW - 1, y: this.baselineY - h, w: barW, h };
    });
    this.saleBars = data.salesByMonth.map((m, i) => {
      const h = (m.total / this.monthlyMax) * innerH;
      return { x: this.pad + i * slot + slot / 2 + 1, y: this.baselineY - h, w: barW, h };
    });
    this.monthLabels = data.purchasesByMonth.map((m, i) => ({
      x: this.pad + i * slot + slot / 2,
      text: m.monthName.slice(0, 3),
    }));

    const n = data.salesLast15Days.length;
    this.dailyPoints = data.salesLast15Days.map((d, i) => ({
      x: this.pad + (n <= 1 ? 0 : (i / (n - 1)) * innerW),
      y: this.baselineY - (d.total / this.dailyMax) * innerH,
      date: d.date,
      total: d.total,
    }));
    this.dailyLine = this.dailyPoints.map((p) => `${p.x.toFixed(1)},${p.y.toFixed(1)}`).join(' ');
  }
}
