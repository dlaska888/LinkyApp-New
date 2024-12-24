import { Component, Input } from '@angular/core';
import { GetLinkDto } from '../../models/dtos/link/getLink.dto';

@Component({
  selector: 'app-link',
  standalone: true,
  imports: [],
  templateUrl: './link.component.html',
  styleUrl: './link.component.scss',
})
export class LinkComponent {
  @Input() link!: GetLinkDto;
}
