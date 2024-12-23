import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PathConstant } from 'app/core/constants/path.constant';
import { AuthService } from 'app/core/services/auth.service';
import { ButtonModule } from 'primeng/button';
import { SidebarItemComponent } from './sidebar-item/sidebar-item.component';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterModule, ButtonModule, CommonModule, SidebarItemComponent],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent {
  PathConst = PathConstant;

  constructor(private auth: AuthService) {}

  logout(): void {
    console.log('Logging out');
    this.auth.logout();
  }
}
