import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '@env/environment';
import {
	LoginUsuario,
	LoginUsuarioResponse,
	RegistroUsuario,
	UpdateUsuario,
	Usuario,
} from '../../shared/usuarios';
import { Action, Store } from '@ngrx/store';
import {
	selectAuthLoading,
	selectAuthRefreshToken,
	selectAuthRegisterSucceded,
	selectAuthToken,
	selectAuthUpdateSucceded,
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
	public username$ = this.store.select(selectAuthUserName);
	public usuario$ = this.store.select(selectAuthUsuario);
	public loading$ = this.store.select(selectAuthLoading);

	public registerSucceded$ = this.store.select(selectAuthRegisterSucceded);
	public loggedInSucceded$ = this.store.select(selectLoggedInSucceded);
	public updateSucceded$ = this.store.select(selectAuthUpdateSucceded);

	constructor(private readonly http: HttpClient) {}

	public isLoggedIn(): boolean {
		const token = localStorage.getItem('authToken');
		return !!token;
	}

	public register(data: RegistroUsuario) {
		return this.http.post<{ succeded: boolean; errors: string[] }>(
			`${this.authUrl}/register`,
			data
		);
	}

	public login(data: LoginUsuario) {
		return this.http.post<LoginUsuarioResponse>(`${this.authUrl}/login`, data);
	}

	public logout(refreshToken: string) {
		return this.http.post<void>(`${this.authUrl}/logout`, { refreshToken });
	}

	public update(data: UpdateUsuario) {
		return this.http.put<Usuario>(`${this.authUrl}/update`, data);
	}

	public validateToken() {
		return this.http.get<{
			valid: boolean;
			username: string;
			usuario: Usuario;
			error: any;
		}>(`${this.authUrl}/validate-token`);
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
