import { createReducer, on } from '@ngrx/store';
import { Usuario } from '../../shared/usuarios';
import { authActions, setToken } from './auth.actions';

export const AUTH_FEATURE_KEY = 'auth';

export interface AuthState {
	token: string | null;
	refreshToken: string | null;
	username: string | null;
	usuario: Usuario | null;
	error: any;
	registerSucceded: boolean | null;
	updateSucceded: boolean | null;
	loading: boolean;
}

export const initialAuthState: AuthState = {
	token: null,
	refreshToken: null,
	username: null,
	usuario: null,
	error: null,
	registerSucceded: null,
	updateSucceded: null,
	loading: false,
};

export const authReducer = createReducer(
	initialAuthState,
	on(setToken, (state, { token, refreshToken }) => ({
		...state,
		token,
		refreshToken
	})),
	on(authActions.register, (state) => ({
		...state,
		loading: true,
		error: null,
	})),
	on(authActions.registerSuccess, (state, action) => ({
		...state,
		registerSucceded: action.response.succeded,
		error: action.response.errors,
		loading: false,
	})),
	on(authActions.registerFailure, (state, action) => ({
		...state,
		registerSucceded: false,
		error: action.error,
		loading: false,
	})),
	on(authActions.loginSuccess, (state, { response }) => ({
		...state,
		token: response.token,
		refreshToken: response.refreshToken,
		username: response.username,
		usuario: response.usuario,
		error: null,
		loading: false,
	})),
	on(authActions.loginFailure, (state, action) => ({
		...state,
		error: action.error,
		loading: false,
	})),
	on(authActions.logout, () => initialAuthState),
	on(authActions.updateRefreshToken, (state, action) => ({
		...state,
		loading: true,
		error: null,
	})),
	on(authActions.refreshTokenUpdated, (state, action) => ({
		...state,
		token: action.response.token,
		refreshToken: action.response.refreshToken,
		username: action.response.username,
		usuario: action.response.usuario,
		error: null,
		loading: false,
	})),
	on(authActions.refreshTokenUpdateFailure, (state, action) => ({
		...state,
		error: action.error,
		loading: false,
	})),
	on(authActions.tokenValidated, (state, action) => ({
		...state,
		username: action.username,
		usuario: action.usuario,
		error: action.error,
		loading: false,
	})),
	on(authActions.tokenValidationFailure, (state, action) => ({
		...state,
		token: null,
		refreshToken: null,
		username: null,
		usuario: null,
		error: action.error,
		loading: false,
	})),
	on(authActions.updateUsuario, (state) => ({
		...state,
		loading: true,
		error: null,
	})),
	on(authActions.updateUsuarioSuccess, (state, action) => ({
		...state,
		usuario: action.usuario,
		updateSucceded: true,
		error: null,
		loading: false,
	})),
	on(authActions.updateUsuarioFailure, (state, action) => ({
		...state,
		updateSucceded: false,
		error: action.error,
		loading: false,
	}))
);
