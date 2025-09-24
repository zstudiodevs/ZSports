import { createReducer, on } from '@ngrx/store';
import { Usuario } from '../../shared/usuarios';
import { authActions } from './auth.actions';

export const AUTH_FEATURE_KEY = 'auth';

export interface AuthState {
	token: string | null;
	refreshToken: string | null;
	userName: string | null;
	usuario: Usuario | null;
	error: any;
}

export const initialAuthState: AuthState = {
	token: null,
	refreshToken: null,
	userName: null,
	usuario: null,
	error: null,
};

export const authReducer = createReducer(
	initialAuthState,
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
