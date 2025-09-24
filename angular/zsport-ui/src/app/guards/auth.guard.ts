import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from '@services/auth.service';
import { Store } from '@ngrx/store';
import { setToken } from '../../state/auth/auth.actions';
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
			switchMap((token) => {
				if (token) {
					return of(true);
				}

				// Buscar token en localStorage
				const localToken = localStorage.getItem('token');
				const localRefreshToken = localStorage.getItem('refreshToken');
				if (localToken && localRefreshToken) {
					// Registrar el token en el store
					this.store.dispatch(setToken({ token: localToken, refreshToken: localRefreshToken }));
					return of(true);
				}

				// Si no hay token, revisa refreshToken
				return this.authService.refreshToken$.pipe(
					take(1),
					switchMap((refreshToken) => {
						if (refreshToken) {
							// Intenta refrescar el token
							return this.authService.refreshToken(refreshToken).pipe(
								map((response) => {
									// Si el refresh fue exitoso, permite acceso
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
	}
}
