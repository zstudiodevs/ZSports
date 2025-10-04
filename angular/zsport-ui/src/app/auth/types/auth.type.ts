export interface User {
	id: string;
	nombre: string;
	apellido: string;
	email: string;
	activo: boolean;
	roles: string[];
}

export interface LoginRequest {
	email: string;
	password: string;
}

export interface LoginResponse {
	token: string;
	refreshToken: string;
	username: string;
	usuario: User;
}

export interface UpdateUserRequest {
	id: string;
	nombre: string;
	apellido: string;
	email: string;
}

export interface ValidateTokenResponse {
	valid: boolean;
	username: string;
	usuario: User;
	error?: string;
}

export type AuthState = {
	token: string | null;
	refreshToken: string | null;
	username: string | null;
	user: User | null;
	isAuthenticated: boolean;
};
