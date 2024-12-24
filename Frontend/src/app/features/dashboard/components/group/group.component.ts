import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, DestroyRef, Input } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LinkyApiConstant } from 'app/core/constants/linkyApi.constant';
import { PagedResults } from 'app/core/models/dtos/pagedResults';
import { MenuItem } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import { ToastModule } from 'primeng/toast';
import { finalize } from 'rxjs';
import { CreateGroupComponent } from '../../forms/create-group/create-group.component';
import { GetGroupDto } from '../../models/dtos/group/getGroup.dto';
import { GetLinkDto } from '../../models/dtos/link/getLink.dto';
import { DialogBasicDemo } from '../form-modal/form-modal.component';
import { LinkComponent } from '../link/link.component';

@Component({
  selector: 'app-group',
  standalone: true,
  imports: [
    CommonModule,
    LinkComponent,
    ToastModule,
    MenuModule,
    DialogBasicDemo,
    CreateGroupComponent,
  ],
  templateUrl: './group.component.html',
  styleUrl: './group.component.scss',
})
export class GroupComponent {
  @Input() group!: GetGroupDto;
  links: GetLinkDto[] = [];
  menuItems: MenuItem[] = [];
  loading = true;

  createGroupDialogVisible = false;

  constructor(
    private http: HttpClient,
    private drf: DestroyRef,
  ) {}

  ngOnInit() {
    this.getLinks();
    this.menuItems = [
      {
        label: 'Add',
        icon: 'pi pi-plus',
        command: () => (this.createGroupDialogVisible = true),
      },
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => {
          console.log('Edit');
        },
      },
      {
        label: 'Share',
        icon: 'pi pi-share-alt',
        command: () => {
          console.log('Share');
        },
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {
          console.log('Delete');
        },
      },
    ];
  }

  private getLinks() {
    this.http
      .get<PagedResults<GetLinkDto>>(LinkyApiConstant.LINK(this.group.id))
      .pipe(
        takeUntilDestroyed(this.drf),
        finalize(() => (this.loading = false)),
      )
      .subscribe((res) => {
        this.links = res.items;
      });
  }
}
