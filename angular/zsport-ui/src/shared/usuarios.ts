export interface Usuario {
	id: string;
	nombre: string;
	apellido: string;
	email: string;
	activo: boolean;
	roles: string[];
}

export interface LoginUsuario {
	email: string;
	password: string;
}

export interface LoginUsuarioResponse {
	token: string;
	refreshToken: string;
	username: string;
	usuario: Usuario;
}

export interface RegistroUsuario {
	userName: string;
	email: string;
	password: string;
}

export interface RegistroUsuarioResponse {
	succeded: boolean;
	errors: string[];
}

export interface UpdateUsuario {
	id: string;
	nombre: string;
	apellido: string;
	email: string;
}
