import { LinkyApiService } from '../../../core/services/linkyApi.service';
import { Component, DestroyRef, inject } from '@angular/core';
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
import { passwordMatchValidator } from '../../../core/validators/passwordMatch.validator';
import { passwordValidator } from '../../../core/validators/password.validator';
import { AuthService } from '../../../core/services/auth.service';
import { HttpResponse } from '@angular/common/http';
import { TokenDto } from '../models/tokenDto';
import { Router, RouterModule } from '@angular/router';
import { filter, finalize } from 'rxjs';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PathConstant } from '../../../core/constants/path.constant';
import { TransConstant } from '../../../core/constants/trans.constant';

@Component({
  selector: 'app-register',
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
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  pathConst = PathConstant;

  constructor(
    private api: LinkyApiService,
    private socialAuth: SocialAuthService,
    private authService: AuthService,
    private router: Router
  ) {}

  registerForm = new FormGroup(
    {
      userName: new FormControl(null, [
        Validators.required,
        Validators.maxLength(255),
      ]),
      email: new FormControl(null, [
        Validators.required,
        Validators.maxLength(255),
        Validators.email,
      ]),
      password: new FormControl(null, [
        Validators.required,
        passwordValidator(),
      ]),
      confirmPassword: new FormControl(null, [Validators.required]),
    },
    passwordMatchValidator('password', 'confirmPassword')
  );

  registerLoading: boolean = false;
  googleLoading: boolean = false;

  private destroyRef = inject(DestroyRef);
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

  onSubmit() {
    this.registerLoading = true;
    this.api
      .register({
        userName: this.registerForm.value.userName!,
        email: this.registerForm.value.email!,
        password: this.registerForm.value.password!,
        confirmPassword: this.registerForm.value.confirmPassword!,
      })
      .pipe(finalize(() => (this.registerLoading = false)))
      .subscribe((response: HttpResponse<TokenDto>) => {
        if (!response.ok || !response.body) {
          console.error(response);
          return;
        }
        this.authService.setTokens(response.body);
        this.tryNavigateToDashboard();
      });
  }

  getPasswordErrors() {
    const control = this.registerForm.get('password');
    return Object.keys(TransConstant.PASSWORD).map((key) => {
      const transKey = key as keyof typeof TransConstant.PASSWORD;
      return {
        message: TransConstant.PASSWORD[transKey],
        isSet: control?.hasError(TransConstant.PASSWORD[transKey]),
      };
    });
  }

  private tryNavigateToDashboard() {
    this.authService.tryAuthenticateUser().then((isAuthenticated) => {
      if (isAuthenticated) {
        this.router.navigate([PathConstant.DASHBOARD]);
      }
    });
  }
}
