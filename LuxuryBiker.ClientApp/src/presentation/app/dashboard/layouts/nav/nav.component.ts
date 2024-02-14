import { Component, OnInit } from '@angular/core';
import { UserLoggedModel } from '@domain/authentication/models/user-logged.model';
import { AuthService } from '@services/auth/auth.service';
import { BehaviorSubject } from 'rxjs';


@Component({
  selector: '[layout-nav]',
  standalone: true,
  imports: [],
  templateUrl: './nav.component.html',
  styles: ``
})
export class NavComponent implements OnInit {

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
