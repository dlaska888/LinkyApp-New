import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, DestroyRef, Input } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { LinkyApiConstant } from 'app/core/constants/linkyApi.constant';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { catchError, EMPTY, finalize } from 'rxjs';
import { GetGroupDto } from '../../models/dtos/group/getGroup.dto';

@Component({
  selector: 'app-create-group',
  templateUrl: './create-group.component.html',
  styleUrls: ['./create-group.component.scss'],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    CardModule,
    FloatLabelModule,
    InputTextModule,
    ButtonModule,
  ],
  standalone: true,
})
export class CreateGroupComponent {
  @Input() onSuccess?: () => void;
  @Input() onFail?: () => void;

  createGroupForm!: FormGroup;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private drf: DestroyRef,
  ) {}

  ngOnInit() {
    this.createGroupForm = this.fb.group({
      name: [
        null,
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(255),
        ],
      ],
      description: [
        null,
        [
          Validators.minLength(3),
          Validators.maxLength(255),
        ],
      ],
    });
  }

  onSubmit(): void {
    if (!this.createGroupForm.invalid) {
      console.error('Form is invalid');
      return;
    }
    const createGroupDto = this.createGroupForm.value;
    this.loading = true;
    this.http
      .post<GetGroupDto>(LinkyApiConstant.GROUP, createGroupDto)
      .pipe(
        takeUntilDestroyed(this.drf),
        catchError((error) => {
          this.onFail?.();
          console.error('Error creating group', error);
          return EMPTY;
        }),
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe((group) => {
        this.onSuccess?.();
        console.log('Group created successfully', group.name);
      });
  }
}
