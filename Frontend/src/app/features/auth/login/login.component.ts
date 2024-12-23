import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, DestroyRef, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { LinkyApiConstant } from 'app/core/constants/linkyApi.constant';
import { setPublicApi } from 'app/core/utils/setPublicApi';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { finalize } from 'rxjs';
import { PathConstant } from '../../../core/constants/path.constant';
import { AuthService } from '../../../core/services/auth.service';
import { GoogleLoginComponent } from '../components/google-login/google-login.component';
import { TokenDto } from '../models/tokenDto';
import { LoginDto } from './models/loginDto';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    CardModule,
    CommonModule,
    FloatLabelModule,
    RouterModule,
    ProgressSpinnerModule,
    GoogleLoginComponent,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements OnInit {
  pathConst = PathConstant;

  loginLoading: boolean = false;
  loginForm = new FormGroup({
    userName: new FormControl(null, [
      Validators.required,
      Validators.maxLength(255),
    ]),
    password: new FormControl(null, [
      Validators.required,
      Validators.maxLength(255),
    ]),
  });

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private destroyRef: DestroyRef,
  ) {}

  ngOnInit(): void {
    this.router.navigate([PathConstant.DASHBOARD]);
  }

  onSubmit() {
    this.loginLoading = true;
    this.http
      .post<TokenDto>(
        LinkyApiConstant.LOGIN,
        {
          userName: this.loginForm.value.userName!,
          password: this.loginForm.value.password!,
        } as LoginDto,
        {
          context: setPublicApi(),
        },
      )
      .pipe(
        finalize(() => (this.loginLoading = false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe((data: TokenDto) => {
        this.authService.setTokens(data);
        this.router.navigate([PathConstant.DASHBOARD]);
      });
  }
}
