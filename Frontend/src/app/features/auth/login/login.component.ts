import { Component, OnDestroy, OnInit, DestroyRef } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { FloatLabelModule } from 'primeng/floatlabel';
import {
  SocialLoginModule,
  GoogleSigninButtonModule,
} from '@abacritt/angularx-social-login';
import { SocialAuthService } from '@abacritt/angularx-social-login';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpResponse } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { filter, finalize } from 'rxjs';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { PathConstant } from '../../../core/constants/path.constant';
import { AuthService } from '../../../core/services/auth.service';
import { TokenDto } from '../models/tokenDto';
import { LinkyApiService } from '../../../core/services/linkyApi.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

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
    SocialLoginModule,
    GoogleSigninButtonModule,
    RouterModule,
    ProgressSpinnerModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements OnInit, OnDestroy {
  pathConst = PathConstant;

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

  loginLoading: boolean = false;
  googleLoading: boolean = false;

  constructor(
    private api: LinkyApiService,
    private socialAuth: SocialAuthService,
    private authService: AuthService,
    private router: Router,
    private destroyRef: DestroyRef
  ) {}

  ngOnInit() {
    this.tryNavigateToDashboard();
    this.socialAuth.authState
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        filter((user) => user !== null)
      )
      .subscribe((user) => {
        this.googleLoading = true;
        this.api
          .googleLogin({
            provider: user.provider,
            idToken: user.idToken,
          })
          .pipe(finalize(() => (this.googleLoading = false)))
          .subscribe(async (response: HttpResponse<TokenDto>) => {
            if (!response.ok || !response.body) {
              console.error(response);
              return;
            }
            this.authService.setTokens(response.body);
            this.tryNavigateToDashboard();
          });
      });
  }

  ngOnDestroy() {
    this.socialAuth.signOut();
  }

  onSubmit() {
    this.loginLoading = true;
    this.api
      .login({
        userName: this.loginForm.value.userName!,
        password: this.loginForm.value.password!,
      })
      .pipe(finalize(() => (this.loginLoading = false)))
      .subscribe((response: HttpResponse<TokenDto>) => {
        if (!response.ok || !response.body) {
          console.error(response);
          return;
        }
        this.authService.setTokens(response.body);
        this.tryNavigateToDashboard();
      });
  }

  private tryNavigateToDashboard() {
    this.router.navigate([PathConstant.DASHBOARD]);
  }
}
