import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest
} from '@angular/common/http';
import { inject } from '@angular/core';
import { UIConfigConstant } from 'app/core/constants/UIConfig.constant';
import { MessageService } from 'primeng/api';
import { catchError, Observable, throwError } from 'rxjs';

export function errorMessageInterceptorFn(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const messageService = inject(MessageService);
  return next(req).pipe(
    catchError((response: HttpErrorResponse) => {
      switch (response.status) {
        case 401:
          break;
        case 400:
          messageService.add({
            severity: 'error',
            detail: response.error.errors
              ? (Object.values(response.error.errors)[0] as string)
              : 'An error occurred while processing your request',
            life: UIConfigConstant.ERROR_MESSAGE_LIFE,
          });
          break;
        default:
          messageService.add({
            severity: 'error',
            detail: response.error?.detail
              ? response.error.detail
              : 'An error occurred while processing your request',
          });
          break;
      }
      return throwError(response);
    })
  );
}
