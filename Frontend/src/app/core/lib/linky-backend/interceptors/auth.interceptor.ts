import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { ApiTokenConstant } from 'app/core/constants/apiToken.constant';
import { AuthService } from 'app/core/services/auth.service';
import { catchError, from, Observable, switchMap, throwError } from 'rxjs';

export function authInterceptorFn(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
): Observable<HttpEvent<unknown>> {
  const authService = inject(AuthService);
  const reqWithToken = () => {
    const { accessToken } = authService.getTokens();
    return req.clone({
      headers: req.headers.set('Authorization', `Bearer ${accessToken}`),
    });
  };

  if (req.context.get(ApiTokenConstant.IS_PUBLIC_API)) {
    return next(req);
  }

  return from(authService.tryAuthenticateUser()).pipe(
    switchMap(() => {
      return next(reqWithToken()).pipe(
        catchError((error) => {
          if (error.status !== 401) return throwError(() => error);
          return from(authService.refreshToken()).pipe(
            switchMap(() => {
              return next(reqWithToken());
            }),
          );
        }),
      );
    }),
  );
}
