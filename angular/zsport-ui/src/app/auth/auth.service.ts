import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '@env/environment';
import {
	LoginRequest,
	LoginResponse,
	UpdateUserRequest,
	User,
	ValidateTokenResponse,
} from './types/auth.type';

@Injectable({ providedIn: 'root' })
export class AuthService {
	private http = inject(HttpClient);
	private apiUrl = `${environment.apiUrl}/usuarios`;

	public login(request: LoginRequest) {
		return this.http.post<LoginResponse>(`${this.apiUrl}/login`, request);
	}

	public logout(refreshToken: string) {
		return this.http.post<void>(`${this.apiUrl}/logout`, { refreshToken });
	}

	public validateToken(token: string) {
		return this.http.get<ValidateTokenResponse>(
			`${this.apiUrl}/validate-token`,
			{
				headers: { Authorization: `Bearer ${token}` },
			}
		);
	}

	public updateProfile(data: UpdateUserRequest) {
		return this.http.put<User>(`${this.apiUrl}/update`, data);
	}
}
