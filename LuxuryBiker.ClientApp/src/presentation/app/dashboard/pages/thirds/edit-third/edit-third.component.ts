import { HttpErrorResponse } from '@angular/common/http';
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
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { catchError, EMPTY, finalize, tap } from 'rxjs';
import { ThirdModel } from '@domain/thirds/models/third.model';
import { ThirdsService } from '@services/thirds/thirds.service';
import { ToastService } from '@shared/toast/toast.service';
import { LoaderComponent } from '@loader/loader.component';

@Component({
  selector: 'app-edit-third',
  standalone: true,
  imports: [ReactiveFormsModule, TranslateModule, RouterLink, LoaderComponent],
  templateUrl: './edit-third.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class EditThirdComponent implements OnInit {
  loading = false;
  processingRequest = false;
  loadError = false;
  private thirdId = 0;

  form = new FormGroup({
    typeId: new FormControl(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
    identification: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    name: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    surnames: new FormControl<string | null>(null),
    cellPhone: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    email: new FormControl<string | null>(null, { validators: [Validators.email] }),
    address: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    active: new FormControl(true, { nonNullable: true }),
  });

  constructor(
    private thirdsService: ThirdsService,
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.thirdId = Number(this.route.snapshot.paramMap.get('id'));
    this.loading = true;

    this.thirdsService
      .getById(this.thirdId)
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
      .subscribe((third) => {
        this.form.patchValue({
          typeId: third.typeId,
          identification: third.identification,
          name: third.name,
          surnames: third.surnames ?? null,
          cellPhone: third.cellPhone,
          email: third.email ?? null,
          address: third.address,
          active: (third.active as unknown as boolean) ?? true,
        });
        this.cdr.markForCheck();
      });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();
    const payload = {
      id: this.thirdId,
      typeId: Number(raw.typeId),
      identification: raw.identification,
      name: raw.name,
      surnames: raw.surnames ?? '',
      cellPhone: raw.cellPhone,
      email: raw.email ?? '',
      address: raw.address,
      active: raw.active,
    } as unknown as ThirdModel;

    this.processingRequest = true;
    this.thirdsService
      .update(payload)
      .pipe(
        tap(() => {
          this.toast.success('Tercero actualizado correctamente.');
          this.router.navigate(['/thirds/list']);
        }),
        finalize(() => {
          this.processingRequest = false;
          this.cdr.markForCheck();
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 400) {
            this.form.setErrors(error.error?.errors ?? { errorUnexpected: true });
            this.form.markAllAsTouched();
          } else if (error.status === 403) {
            this.setFormError('No cuenta con los permisos necesarios para editar terceros.');
          } else if (error.status === 404) {
            this.setFormError('El tercero ya no existe.');
          } else {
            this.setFormError('Ocurrió un error inesperado, intente de nuevo más tarde.');
          }
          this.cdr.markForCheck();
          return EMPTY;
        })
      )
      .subscribe();
  }

  private setFormError(message: string): void {
    this.form.setErrors({ errorUnexpected: true, error: message });
    this.toast.error(message);
  }
}
