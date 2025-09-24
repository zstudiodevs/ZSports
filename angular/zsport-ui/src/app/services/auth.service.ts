import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { LoginUsuario, LoginUsuarioResponse, RegistroUsuario } from '../../shared/usuarios';
import { Action, Store } from '@ngrx/store';
import {
	selectAuthRefreshToken,
	selectAuthRegisterSucceded,
	selectAuthToken,
	selectAuthUserName,
	selectAuthUsuario,
	selectLoggedInSucceded,
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

	public registerSucceded$ = this.store.select(selectAuthRegisterSucceded);
	public loggedInSucceded$ = this.store.select(selectLoggedInSucceded);

	constructor(private readonly http: HttpClient) {}

	public isLoggedIn(): boolean {
		const token = localStorage.getItem('authToken');
		return !!token;
	}

	public register(data: RegistroUsuario) {
		return this.http.post<{succeded: boolean, errors: string[]}>(`${this.authUrl}/register`, data);
	}

	public login(data: LoginUsuario) {
		return this.http.post<LoginUsuarioResponse>(`${this.authUrl}/login`, data);
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
