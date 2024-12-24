import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LinkyApiConstant } from 'app/core/constants/linkyApi.constant';
import { PagedResults } from 'app/core/models/dtos/pagedResults';
import { finalize } from 'rxjs';
import { GroupComponent } from '../../components/group/group.component';
import { GetGroupDto } from '../../models/dtos/group/getGroup.dto';

@Component({
  selector: 'app-my-links',
  standalone: true,
  imports: [CommonModule, GroupComponent],
  templateUrl: './my-links.component.html',
  styleUrl: './my-links.component.scss',
})
export class MyLinksComponent {
  groups: GetGroupDto[] = [];
  loading = true;

  constructor(
    private http: HttpClient,
    private drf: DestroyRef,
  ) {}

  ngOnInit() {
    this.getLinks();
  }

  private getLinks() {
    this.http
      .get<PagedResults<GetGroupDto>>(LinkyApiConstant.GROUP)
      .pipe(
        takeUntilDestroyed(this.drf),
        finalize(() => (this.loading = false)),
      )
      .subscribe((res) => {
        this.groups = res.items;
      });
  }
}
