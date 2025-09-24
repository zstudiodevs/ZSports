import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { LoginUsuarioResponse } from '../../shared/usuarios';
import { Action, Store } from '@ngrx/store';
import {
	selectAuthRefreshToken,
	selectAuthToken,
	selectAuthUserName,
	selectAuthUsuario,
} from '../../state/auth/auth.selectors';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
	private store = inject(Store);
	private authUrl = `${environment.apiUrl}/Usuarios`;

	public token$ = this.store.select(selectAuthToken);
	public refreshToken$ = this.store.select(selectAuthRefreshToken);
	public userName$ = this.store.select(selectAuthUserName);
	public usuario$ = this.store.select(selectAuthUsuario);

	constructor(private readonly http: HttpClient) {}

	public isLoggedIn(): boolean {
		const token = localStorage.getItem('authToken');
		return !!token;
	}

	public login(username: string, password: string) {
		return this.http.post<LoginUsuarioResponse>(`${this.authUrl}/login`, {
			email: username,
			password: password,
		});
	}

	public logout(refreshToken: string) {
		return this.http.post<void>(`${this.authUrl}/logout`, {
			refreshToken,
		});
	}

	public refreshToken(refreshToken: string) {
		return this.http.post<LoginUsuarioResponse>(
			`${this.authUrl}/refresh-token`,
			{
				refreshToken,
			}
		);
	}

	public dispatch(action: Action) {
		this.store.dispatch(action);
	}
}
