import { Component } from '@angular/core';
import { UserLoggedModel } from '@domain/authentication/models/user-logged.model';
import { AuthService } from '@services/auth/auth.service';

@Component({
  selector: '[layout-sidebar]',
  standalone: true,
  imports: [],
  templateUrl: './sidebar.component.html',
  styles: ``
})
export class SidebarComponent {

  userData:UserLoggedModel|null = {} as UserLoggedModel;
  constructor(
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.authService.user$
    .subscribe({
      next:(user) => {
          this.userData = user;
      }
    });
  }
}
