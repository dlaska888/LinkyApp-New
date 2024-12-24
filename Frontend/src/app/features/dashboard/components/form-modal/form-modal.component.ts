import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-form-modal',
  templateUrl: './form-modal.component.html',
  standalone: true,
  imports: [DialogModule, ButtonModule, InputTextModule],
})
export class DialogBasicDemo {
  @Input() visible: boolean = false;
  @Output() visibleChange = new EventEmitter<boolean>();
  @Input() header?: string;
  @Input() description?: string;

  close() {
    this.visible = false;
    this.visibleChange.emit(false);
  }
}
