import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-sidebar-item',
  templateUrl: './sidebar-item.component.html',
  styleUrl: './sidebar-item.component.scss',
  standalone: true,
  imports: [RouterModule, ButtonModule, CommonModule],
})
export class SidebarItemComponent {
  @Input() routerLink!: string;
  @Input() icon!: string;
  @Input() label!: string;
  @Input() pathConst!: string;

  constructor(private router: Router) {}

  isActive(path: string): boolean {
    const currentPath = this.router.url.split('/')[2];
    console.log(currentPath);
    return currentPath === path;
  }
}
