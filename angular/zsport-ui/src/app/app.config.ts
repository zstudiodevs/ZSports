import {
	ApplicationConfig,
	provideZoneChangeDetection,
	isDevMode,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideStore } from '@ngrx/store';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideEffects } from '@ngrx/effects';
import { provideRouterStore } from '@ngrx/router-store';
import { authReducer } from '../state/auth/auth.reducer';
import { AuthEffects } from '../state/auth/auth.effects';
	import { authTokenInterceptorFn } from './interceptors/auth-token.interceptor';

export const appConfig: ApplicationConfig = {
	providers: [
		provideZoneChangeDetection({ eventCoalescing: true }),
		provideAnimationsAsync(),
		provideRouter(routes),
		   provideHttpClient(
			   withInterceptors([
				   authTokenInterceptorFn
			   ])
		   ),
		provideStore({ auth: authReducer }),
		provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
		provideEffects(AuthEffects),
		provideRouterStore(),
	],
};
