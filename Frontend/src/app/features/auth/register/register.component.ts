import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, DestroyRef } from '@angular/core';
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
import { TransConstant } from '../../../core/constants/trans.constant';
import { AuthService } from '../../../core/services/auth.service';
import { GoogleLoginComponent } from '../components/google-login/google-login.component';
import { TokenDto } from '../models/tokenDto';
import { passwordValidator } from '../validators/password.validator';
import { passwordMatchValidator } from '../validators/passwordMatch.validator';
import { RegisterDto } from './models/registerDto';

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
    RouterModule,
    ProgressSpinnerModule,
    GoogleLoginComponent,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  pathConst = PathConstant;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private destroyRef: DestroyRef,
  ) {}

  registerLoading: boolean = false;
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
    passwordMatchValidator('password', 'confirmPassword'),
  );

  ngOnInit() {
    this.router.navigate([PathConstant.DASHBOARD]);
  }

  onSubmit() {
    this.registerLoading = true;
    this.http
      .post<TokenDto>(
        LinkyApiConstant.REGISTER,
        {
          userName: this.registerForm.value.userName!,
          email: this.registerForm.value.email!,
          password: this.registerForm.value.password!,
          confirmPassword: this.registerForm.value.confirmPassword!,
        } as RegisterDto,
        {
          context: setPublicApi(),
        },
      )
      .pipe(
        finalize(() => (this.registerLoading = false)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe((body: TokenDto) => {
        this.authService.setTokens(body);
        this.router.navigate([PathConstant.DASHBOARD]);
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
}
