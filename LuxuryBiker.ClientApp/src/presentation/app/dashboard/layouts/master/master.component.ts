import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { NavComponent } from '../nav/nav.component';
import { FooterComponent } from '../footer/footer.component';

@Component({
  selector: 'app-master',
  standalone: true,
  imports: [ RouterModule, SidebarComponent, NavComponent, FooterComponent ],
  templateUrl: './master.component.html',
  styles: ``
})
export default class MasterComponent {

}
