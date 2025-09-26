import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from '@services/auth.service';
import { Store } from '@ngrx/store';
import { authActions, setToken } from '../../state/auth/auth.actions';
import { switchMap, take, map, catchError } from 'rxjs/operators';

@Injectable({
	providedIn: 'root',
})
export class AuthGuard implements CanActivate {
	constructor(
		private authService: AuthService,
		private router: Router,
		private store: Store
	) {}

	canActivate(): Observable<boolean | UrlTree> {
		return this.authService.token$.pipe(
			take(1),
			switchMap((token) => {
				let tokenToValidate = token;
				if (!tokenToValidate) {
					tokenToValidate = localStorage.getItem('authToken');
				}

				if (tokenToValidate) {
					// Validar token contra el backend
					return this.authService.validateToken().pipe(
						map((res) => {
							if (res && res.valid) {
								this.store.dispatch(authActions.tokenValidated({
									valid: res.valid,
									username: res.username,
									usuario: res.usuario,
									error: res.error
								}));
								return true;
							} else {
								this.store.dispatch(authActions.tokenValidationFailure({ error: res.error }));
								// Si el token no es válido, intentar refresh
								return null;
							}
						}),
						catchError(() => of(null)),
						switchMap((isValid) => {
							if (isValid === true) {
								return of(true);
							}
							// Buscar refreshToken en store o localStorage
							return this.authService.refreshToken$.pipe(
								take(1),
								switchMap((refreshToken) => {
									let refreshToUse = refreshToken || localStorage.getItem('refreshToken');
									if (refreshToUse) {
										return this.authService.refreshToken(refreshToUse).pipe(
											map((response) => {
												return !!response ? true : this.router.createUrlTree(['/']);
											}),
											catchError(() => of(this.router.createUrlTree(['/'])))
										);
									}
									// Si no hay refreshToken, redirige a login
									return of(this.router.createUrlTree(['/']));
								})
							);
						})
					);
				} else {
					// No hay token, buscar refreshToken
					return this.authService.refreshToken$.pipe(
						take(1),
						switchMap((refreshToken) => {
							let refreshToUse = refreshToken || localStorage.getItem('refreshToken');
							if (refreshToUse) {
								return this.authService.refreshToken(refreshToUse).pipe(
									map((response) => {
										return !!response ? true : this.router.createUrlTree(['/']);
									}),
									catchError(() => of(this.router.createUrlTree(['/'])))
								);
							}
							// Si no hay refreshToken, redirige a login
							return of(this.router.createUrlTree(['/']));
						})
					);
				}
			})
		);
	}
}
