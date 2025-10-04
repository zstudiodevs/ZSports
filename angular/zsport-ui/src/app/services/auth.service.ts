import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import {
	LoginUsuario,
	LoginUsuarioResponse,
	RegistroUsuario,
	UpdateUsuario,
	Usuario,
} from '../../shared/usuarios';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
	private authUrl = `${environment.apiUrl}/Usuarios`;

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
}
