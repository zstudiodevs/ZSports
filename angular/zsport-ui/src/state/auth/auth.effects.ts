import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { AuthService } from '@services/auth.service';
import { authActions, setToken } from './auth.actions';
import { filter, map, switchMap, tap, withLatestFrom } from 'rxjs';
import { fetch } from '@ngrx/router-store/data-persistence';

@Injectable()
export class AuthEffects {
	init$ = createEffect(() =>
		this.actions$.pipe(
			ofType('@ngrx/effects/init'),
			map(() => {
				const token = localStorage.getItem('authToken');
				const refreshToken = localStorage.getItem('refreshToken');
				if (token && refreshToken) {
					return setToken({ token, refreshToken });
				} else {
					return { type: 'NO_ACTION' };
				}
			})
		)
	);

	register$ = createEffect(() =>
		this.actions$.pipe(
			ofType(authActions.register),
			fetch({
				run: ({ email, password, userName }) =>
					this.authService.register({ email, password, userName }).pipe(
						filter((response) => response.succeded),
						map((response) => {
							return authActions.registerSuccess({ response });
						})
					),
				onError: (action, error) => {
					return authActions.registerFailure({ error });
				},
			})
		)
	);

	login$ = createEffect(() =>
		this.actions$.pipe(
			ofType(authActions.login),
			fetch({
				run: ({ email, password }) =>
					this.authService.login({ email, password }).pipe(
						filter((response) => !!response),
						map((response) => {
							localStorage.setItem('authToken', response.token);
							localStorage.setItem('refreshToken', response.refreshToken);
							return authActions.loginSuccess({ response });
						})
					),
				onError: (action, error) => {
					localStorage.removeItem('authToken');
					localStorage.removeItem('refreshToken');
					return authActions.loginFailure({ error });
				},
			})
		)
	);

	logout$ = createEffect(() =>
		this.actions$.pipe(
			ofType(authActions.logout),
			fetch({
				run: ({ refreshToken }) =>
					this.authService
						.logout(refreshToken)
						.pipe(map(() => authActions.logoutSuccess())),
				onError: (action, error) => authActions.logoutFailure({ error }),
			})
		)
	);

	updateRefreshToken$ = createEffect(() =>
		this.actions$.pipe(
			ofType(authActions.updateRefreshToken),
			fetch({
				run: ({ refreshToken }) =>
					this.authService.refreshToken(refreshToken).pipe(
						map((response) => {
							localStorage.setItem('authToken', response.token);
							localStorage.setItem('refreshToken', response.refreshToken);
							return authActions.refreshTokenUpdated({ response });
						})
					),
				onError: (action, error) =>
					authActions.refreshTokenUpdateFailure({ error }),
			})
		)
	);

	constructor(private actions$: Actions, private authService: AuthService) {}
}
