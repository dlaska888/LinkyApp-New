import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

export function jsonContentTypeInterceptorFn(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
): Observable<HttpEvent<unknown>> {
  if (req.headers.has('Content-Type')) return next(req);
  const clonedRequest = req.clone({
    headers: req.headers.set('Content-Type', 'application/json'),
  });
  return next(clonedRequest);
}
