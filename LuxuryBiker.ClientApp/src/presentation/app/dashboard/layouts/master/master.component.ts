import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { NavComponent } from '../nav/nav.component';
import { FooterComponent } from '../footer/footer.component';
import { AuthService } from '@services/auth/auth.service';
import { catchError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

declare var jQuery: any; 

@Component({
  selector: 'app-master',
  standalone: true,
  imports: [ RouterModule, SidebarComponent, NavComponent, FooterComponent ],
  templateUrl: './master.component.html',
  styles: ``
})
export default class MasterComponent implements OnInit {
  
  constructor(private authService: AuthService,
    private router: Router) {

    this.authService.getProfileCurrentUser()
    .pipe(
      catchError((error: HttpErrorResponse) => {
        this.router.navigateByUrl("/login");
        throw error;
      })
    )
    .subscribe();
  }

  ngOnInit(): void {
    
    let body = jQuery('body');
    let sidebar = jQuery('.sidebar');

    // Close other submenu in sidebar on opening any
    sidebar.on('show.bs.collapse', '.collapse', function() {
      sidebar.find('.collapse.show').collapse('hide');
    });


    //Change sidebar and content-wrapper height
    jQuery('[data-toggle="minimize"]').on("click", function() {
      if ((body.hasClass('sidebar-toggle-display')) || (body.hasClass('sidebar-absolute'))) {
        body.toggleClass('sidebar-hidden');
      } else {
        body.toggleClass('sidebar-icon-only');
      }
    });
  }
}
