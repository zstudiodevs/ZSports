import { createReducer, on } from '@ngrx/store';
import { Usuario } from '../../shared/usuarios';
import { authActions, setToken } from './auth.actions';

export const AUTH_FEATURE_KEY = 'auth';

export interface AuthState {
	token: string | null;
	refreshToken: string | null;
	userName: string | null;
	usuario: Usuario | null;
	error: any;
	registerSucceded: boolean | null;
}

export const initialAuthState: AuthState = {
	token: null,
	refreshToken: null,
	userName: null,
	usuario: null,
	error: null,
	registerSucceded: null
};

export const authReducer = createReducer(
	initialAuthState,
	on(setToken, (state, { token, refreshToken }) => ({
		...state,
		token,
		refreshToken
	})),
	on(authActions.registerSuccess, (state, action) => ({
		...state,
		registerSucceded: action.response.succeded,
		error: action.response.errors
	})),
	on(authActions.loginSuccess, (state, { response }) => ({
		...state,
		token: response.token,
		refreshToken: response.refreshToken,
		userName: response.userName,
		usuario: response.usuario,
	})),
	on(authActions.loginFailure, (state, action) => ({
		...state,
		error: action.error,
	})),
	on(authActions.logout, () => initialAuthState)
);
