import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthStore } from '../store/auth.store';
import { AuthService } from '../auth.service';
import { catchError, map, of } from 'rxjs';
import { SnackbarService } from '@app/shared/snackbar/services/snackbar.service';

export const authGuard: CanActivateFn = (route, state) => {
	const authStore = inject(AuthStore);
	const authService = inject(AuthService);
	const snackbarService = inject(SnackbarService);
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
					snackbarService.open({
						message: 'Sesión expirada. Por favor, inicia sesión de nuevo.',
						duration: 3000,
						type: 'warning',
					});
					return router.createUrlTree(['/']);
				}
			}),
			catchError((error) => {
				authStore.resetState();
				snackbarService.open({
					message:
						'Error al validar la sesión. Por favor, inicia sesión de nuevo.',
					duration: 3000,
					type: 'warning',
				});
				return of(router.createUrlTree(['/']));
			})
		);
	} else {
		authStore.resetState();
		snackbarService.open({
			message: 'No estás autenticado. Por favor, inicia sesión.',
			duration: 3000,
			type: 'warning',
		});
		return router.createUrlTree(['/']);
	}
};
