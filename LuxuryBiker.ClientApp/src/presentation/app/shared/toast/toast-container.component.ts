import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ToastService } from './toast.service';

@Component({
  selector: 'app-toast-container',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="toast-stack">
      @for (toast of toastService.toasts$ | async; track toast.id) {
        <div class="toast-item" [class.toast-success]="toast.kind === 'success'"
          [class.toast-error]="toast.kind === 'error'" [class.toast-info]="toast.kind === 'info'"
          role="alert">
          <span class="toast-message">{{ toast.message }}</span>
          <button type="button" class="toast-close" (click)="toastService.dismiss(toast.id)"
            aria-label="Cerrar">&times;</button>
        </div>
      }
    </div>
  `,
  styles: [
    `
      .toast-stack {
        position: fixed;
        top: 1rem;
        right: 1rem;
        z-index: 1080;
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
        max-width: 22rem;
      }
      .toast-item {
        display: flex;
        align-items: flex-start;
        gap: 0.75rem;
        padding: 0.75rem 1rem;
        border-radius: 6px;
        color: #fff;
        box-shadow: 0 6px 20px rgba(0, 0, 0, 0.18);
        font-size: 0.875rem;
      }
      .toast-success { background: #46c35f; }
      .toast-error { background: #d9534f; }
      .toast-info { background: #0b94f7; }
      .toast-message { flex: 1; }
      .toast-close {
        background: transparent;
        border: 0;
        color: inherit;
        font-size: 1.1rem;
        line-height: 1;
        cursor: pointer;
        opacity: 0.85;
      }
    `,
  ],
})
export class ToastContainerComponent {
  constructor(public toastService: ToastService) {}
}
