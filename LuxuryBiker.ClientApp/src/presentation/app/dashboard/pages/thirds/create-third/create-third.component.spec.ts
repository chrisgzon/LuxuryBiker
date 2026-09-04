import { ComponentFixture, TestBed } from '@angular/core/testing';
import { importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { TranslateModule } from '@ngx-translate/core';
import {
  thirdCreateUseCaseProvider,
  getThirdsUseCaseProvider,
  thirdsImplementationRepositoryProvider,
} from '@data/thirds';

import CreateThirdComponent from './create-third.component';

describe('CreateThirdComponent', () => {
  let component: CreateThirdComponent;
  let fixture: ComponentFixture<CreateThirdComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateThirdComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting(),
        importProvidersFrom(TranslateModule.forRoot()),
        thirdCreateUseCaseProvider,
        getThirdsUseCaseProvider,
        thirdsImplementationRepositoryProvider,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateThirdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('starts with an invalid form', () => {
    expect(component.form.valid).toBeFalse();
  });
});
