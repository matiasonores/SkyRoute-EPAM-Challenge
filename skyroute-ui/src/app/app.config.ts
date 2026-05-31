import {
  APP_INITIALIZER,
  ApplicationConfig,
  provideBrowserGlobalErrorListeners
} from '@angular/core';

import {
  provideRouter,
  withComponentInputBinding,
  withEnabledBlockingInitialNavigation
} from '@angular/router';

import {
  provideHttpClient,
  withFetch,
  withInterceptorsFromDi
} from '@angular/common/http';

import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import { ApplicationStartupService } from './core/services/application-startup.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),

    provideRouter(
      routes,
      withEnabledBlockingInitialNavigation(),
      withComponentInputBinding()
    ),

    provideHttpClient(
      withFetch(),
      withInterceptorsFromDi()
    ),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApplication,
      deps: [ApplicationStartupService],
      multi: true
    },
    provideAnimations()
  ]
};

function initializeApplication(
  startupService: ApplicationStartupService
) {
  return () =>
    startupService.initialize();
}