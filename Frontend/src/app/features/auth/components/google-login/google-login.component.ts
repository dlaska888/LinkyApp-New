import {
  GoogleSigninButtonModule,
  SocialAuthService,
  SocialLoginModule,
} from '@abacritt/angularx-social-login';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, DestroyRef, EventEmitter, Output } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { LinkyApiConstant } from 'app/core/constants/linkyApi.constant';
import { PathConstant } from 'app/core/constants/path.constant';
import { AuthService } from 'app/core/services/auth.service';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { filter, finalize } from 'rxjs';
import { ExternalAuthDto } from '../../models/externalAuthDto';
import { TokenDto } from '../../models/tokenDto';

@Component({
  selector: 'app-google-login',
  standalone: true,
  imports: [
    SocialLoginModule,
    GoogleSigninButtonModule,
    ProgressSpinnerModule,
    CommonModule,
  ],
  templateUrl: './google-login.component.html',
  styleUrl: './google-login.component.scss',
})
export class GoogleLoginComponent {
  googleLoading: boolean = false;

  @Output()
  googleLoadinChange = new EventEmitter<boolean>(false);

  constructor(
    private authService: AuthService,
    private socialAuth: SocialAuthService,
    private router: Router,
    private http: HttpClient,
    private drf: DestroyRef,
  ) {}

  ngOnInit() {
    this.socialAuth.authState
      .pipe(
        filter((user) => user !== null),
        takeUntilDestroyed(this.drf),
      )
      .subscribe((user) => {
        this.googleLoading = true;
        this.http
          .post<TokenDto>(LinkyApiConstant.GOOGLE_LOGIN, {
            provider: user.provider,
            idToken: user.idToken,
          } as ExternalAuthDto)
          .pipe(
            finalize(() => (this.googleLoading = false)),
            takeUntilDestroyed(this.drf),
          )
          .subscribe((body: TokenDto) => {
            this.authService.setTokens(body);
            this.router.navigate([PathConstant.DASHBOARD]);
          });
      });
  }

  ngOnDestroy() {
    this.socialAuth.signOut();
  }
}
