import { Component, OnInit } from '@angular/core';
import { UserLoggedModel } from '@domain/authentication/models/user-logged.model';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthService } from '../../../services/auth/auth.service';
import { RouterLink } from '@angular/router';


@Component({
  selector: '[layout-nav]',
  standalone: true,
  imports: [TranslateModule, RouterLink],
  templateUrl: './nav.component.html',
  styles: ``
})
export class NavComponent implements OnInit {

  userData:UserLoggedModel|null = {} as UserLoggedModel;

  constructor(
    public authService: AuthService,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {
    this.authService.user$
    .subscribe({
      next:(user) => {
          this.userData = user;
      }
    });
  }

  logout() {
    this.authService.logout()
  }
  
  onLanguageChange(selectedLanguage: string) {
    this.translateService.use(selectedLanguage)
  }
}
