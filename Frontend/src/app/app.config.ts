import {
  GoogleLoginProvider,
  SocialAuthServiceConfig,
} from '@abacritt/angularx-social-login';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import {
  ApplicationConfig,
  isDevMode,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideRouter } from '@angular/router';
import { provideServiceWorker } from '@angular/service-worker';
import { RxFormBuilder } from '@rxweb/reactive-form-validators';
import { ConfirmationService, MessageService } from 'primeng/api';
import { environment } from '../environments/environment';
import { routes } from './app.routes';
import { authInterceptorFn } from './core/lib/linky-backend/interceptors/auth.interceptor';
import { errorMessageInterceptorFn } from './core/lib/linky-backend/interceptors/error-message.interceptor';
import { jsonContentTypeInterceptorFn } from './core/lib/linky-backend/interceptors/json-content-type.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    MessageService,
    ConfirmationService,
    RxFormBuilder,
    provideHttpClient(
      withInterceptors([
        jsonContentTypeInterceptorFn,
        authInterceptorFn,
        errorMessageInterceptorFn,
      ]),
    ),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimations(),
    provideServiceWorker('ngsw-worker.js', {
      enabled: !isDevMode(),
      registrationStrategy: 'registerWhenStable:30000',
    }),
    provideAnimationsAsync(),
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        lang: 'en',
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(environment.google.clientId, {
              oneTapEnabled: false,
              prompt: 'consent',
            }),
          },
        ],
        onError: (err) => {
          console.error(err);
        },
      } as SocialAuthServiceConfig,
    },
  ],
};
