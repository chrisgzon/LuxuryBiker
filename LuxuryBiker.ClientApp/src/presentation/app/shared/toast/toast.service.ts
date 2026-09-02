import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export type ToastKind = 'success' | 'error' | 'info';

export interface Toast {
  id: number;
  kind: ToastKind;
  message: string;
}

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private readonly defaultTimeoutMs = 4500;
  private nextId = 1;
  private readonly toasts = new BehaviorSubject<Toast[]>([]);

  readonly toasts$: Observable<Toast[]> = this.toasts.asObservable();

  success(message: string): void {
    this.show('success', message);
  }

  error(message: string): void {
    this.show('error', message);
  }

  info(message: string): void {
    this.show('info', message);
  }

  show(kind: ToastKind, message: string): void {
    const toast: Toast = { id: this.nextId++, kind, message };
    this.toasts.next([...this.toasts.value, toast]);
    setTimeout(() => this.dismiss(toast.id), this.defaultTimeoutMs);
  }

  dismiss(id: number): void {
    this.toasts.next(this.toasts.value.filter((toast) => toast.id !== id));
  }
}
