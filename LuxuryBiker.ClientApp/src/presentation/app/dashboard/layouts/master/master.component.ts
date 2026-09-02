import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { NavComponent } from '../nav/nav.component';
import { FooterComponent } from '../footer/footer.component';
import { AuthService } from '../../../services/auth/auth.service';
import { SettingsComponent } from "../settings/settings.component";
import { ToastContainerComponent } from "../../../shared/toast/toast-container.component";

@Component({
  selector: 'app-master',
  standalone: true,
  imports: [RouterModule, SidebarComponent, NavComponent, FooterComponent, SettingsComponent, ToastContainerComponent],
  templateUrl: './master.component.html',
  styles: ``
})
export default class MasterComponent implements OnInit {
  
  constructor(private authService: AuthService) {

    this.authService.getProfileCurrentUser()
    .subscribe();
  }

  ngOnInit(): void {
    
    melodyJs.misc();
    melodyJs.offCanvas();
    melodyJs.settings();
  }

  ngAfterViewInit(): void {
  }
}
