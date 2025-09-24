import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AUTH_FEATURE_KEY, AuthState } from './auth.reducer';

// Selector para el estado completo de auth
export const selectAuthState =
	createFeatureSelector<AuthState>(AUTH_FEATURE_KEY);

// Selector para el token
export const selectAuthToken = createSelector(
	selectAuthState,
	(state) => state.token
);

// Selector para el refreshToken
export const selectAuthRefreshToken = createSelector(
	selectAuthState,
	(state) => state.refreshToken
);

// Selector para el userName
export const selectAuthUserName = createSelector(
	selectAuthState,
	(state) => state.userName
);

// Selector para el usuario
export const selectAuthUsuario = createSelector(
	selectAuthState,
	(state) => state.usuario
);

// Selector para el error
export const selectAuthError = createSelector(
	selectAuthState,
	(state) => state.error
);

export const selectLoggedInSucceded = createSelector(
	selectAuthState,
	(state) => ({
		succeded: !!state.token,
		errors: state.error
	})
);

// Selector para el estado de registro
export const selectAuthRegisterSucceded = createSelector(
	selectAuthState,
	(state) => ({ succeded: state.registerSucceded, errors: state.error }) 
);
