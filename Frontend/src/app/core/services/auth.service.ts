import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { BehaviorSubject, filter, finalize, firstValueFrom, take } from 'rxjs';
import { TokenDto } from '../../features/auth/models/tokenDto';
import { ApiTokenConstant } from '../constants/apiToken.constant';
import { LinkyApiConstant } from '../constants/linkyApi.constant';
import { LocalStorageConstant } from '../constants/localStorage.constant';
import { PathConstant } from '../constants/path.constant';
import { setPublicApi } from '../utils/setPublicApi';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private tokenRefreshingSubject = new BehaviorSubject<boolean>(false);

  constructor(
    private http: HttpClient,
    private router: Router,
  ) {}

  getTokens(): TokenDto {
    return {
      accessToken:
        localStorage.getItem(LocalStorageConstant.ACCESS_TOKEN) || '',
      refreshToken:
        localStorage.getItem(LocalStorageConstant.REFRESH_TOKEN) || '',
    };
  }

  setTokens(tokens: TokenDto): void {
    localStorage.setItem(LocalStorageConstant.ACCESS_TOKEN, tokens.accessToken);
    localStorage.setItem(
      LocalStorageConstant.REFRESH_TOKEN,
      tokens.refreshToken,
    );
  }

  async tryAuthenticateUser(): Promise<boolean> {
    let { accessToken, refreshToken } = this.getTokens();
    if (!accessToken || !refreshToken) return Promise.resolve(false);

    if (!this.isTokenExpired(accessToken)) return Promise.resolve(true);

    await this.refreshToken();

    return Promise.resolve(true);
  }

  async refreshToken(): Promise<void> {
    if (await this.isTokenRefreshing()) return Promise.resolve(); // No need to refresh token
    this.tokenRefreshingSubject.next(true);

    const { refreshToken } = this.getTokens();

    return new Promise((resolve, reject) => {
      this.http
        .post<TokenDto>(
          LinkyApiConstant.REFRESH_TOKEN,
          JSON.stringify(refreshToken),
          {
            context: setPublicApi(),
          },
        )
        .pipe(finalize(() => this.tokenRefreshingSubject.next(false)))
        .subscribe({
          next: (body) => {
            this.setTokens(body);
            console.log('Tokens refreshed');
            return resolve();
          },
          error: (error) => {
            console.error(error);
            this.redirectToLogin();
            return reject(error);
          },
        });
    });
  }

  logout(): void {
    localStorage.clear();
    this.redirectToLogin();
  }

  private redirectToLogin(): void {
    localStorage.clear();
    console.log('Redirecting to login page');
    this.router.navigate([PathConstant.LOGIN]);
  }

  private isTokenExpired(accessToken: string) {
    try {
      const decoded = jwtDecode<JwtPayload>(accessToken);
      const exp = decoded.exp! * 1000;
      const maxExp = Date.now() + ApiTokenConstant.REFRESH_OFFSET * 1000;
      if (maxExp > exp) console.log('Token expired!');
      return maxExp > exp;
    } catch (error) {
      console.error(error);
      return false;
    }
  }

  private async isTokenRefreshing(): Promise<boolean> {
    if (!this.tokenRefreshingSubject.getValue()) return false;
    await firstValueFrom(
      this.tokenRefreshingSubject.asObservable().pipe(
        filter((refreshing) => refreshing === false),
        take(1),
      ),
    );
    return true;
  }
}
