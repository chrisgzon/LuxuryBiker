import { Component } from '@angular/core';
import { UserLoggedModel, ROLES } from '@domain/authentication/models/user-logged.model';
import { AuthService } from '../../../services/auth/auth.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: '[layout-sidebar]',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './sidebar.component.html',
  styles: ``
})
export class SidebarComponent {

  userData:UserLoggedModel|null = {} as UserLoggedModel;
  constructor(
    public authService: AuthService
  ) {}

  /** Los módulos operativos solo se muestran a roles con permisos de gestión. */
  get canManage(): boolean {
    return this.authService.hasAnyRole(ROLES.administrator, ROLES.seller);
  }

  ngOnInit(): void {
    this.authService.user$
    .subscribe({
      next:(user) => {
          this.userData = user;
      }
    });
  }
}
