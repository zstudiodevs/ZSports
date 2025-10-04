import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthStore } from '../store/auth.store';
import { AuthService } from '../auth.service';
import { catchError, map, of } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
	const authStore = inject(AuthStore);
	const authService = inject(AuthService);
	const router = inject(Router);

	if (authStore.isAuthenticated$()) {
		return authService.validateToken(authStore.token$()!).pipe(
			map((response) => {
				if (response.valid) {
					authStore.setState({
						username: response.username,
						user: response.usuario,
						isAuthenticated: true,
					});
					return true;
				} else {
					authStore.resetState();
					return router.createUrlTree(['/']);
				}
			}),
			catchError((error) => {
				authStore.resetState();
				return of(router.createUrlTree(['/']));
			})
		);
	} else {
		authStore.resetState();
		return router.createUrlTree(['/']);
	}
};
