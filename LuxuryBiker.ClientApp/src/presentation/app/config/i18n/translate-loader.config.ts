import { HttpClient } from "@angular/common/http";
import { TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

export function createTranslateLoader(http: HttpClient) {
    return new TranslateHttpLoader(http, './../../../assets/i18n/', '.json');
}

export function provideTranslation() {
    return {
        defaultLanguage: 'es',
        loader: {
            provide: TranslateLoader,
            useFactory: (createTranslateLoader),
            deps: [HttpClient]
        },
    }
}