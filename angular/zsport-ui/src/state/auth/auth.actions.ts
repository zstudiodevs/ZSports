import { createAction, createActionGroup, emptyProps, props } from '@ngrx/store';
import { LoginUsuarioResponse, RegistroUsuarioResponse } from '../../shared/usuarios';

export const authActions = createActionGroup({
	source: 'Auth',
	events: {
		Register: props<{ email: string; password: string; userName: string }>(),
		RegisterSuccess: props<{ response: RegistroUsuarioResponse }>(),
		RegisterFailure: props<{ error: any }>(),
		Login: props<{ email: string; password: string }>(),
		LoginSuccess: props<{ response: LoginUsuarioResponse }>(),
		LoginFailure: props<{ error: any }>(),
		Logout: props<{ refreshToken: string }>(),
		LogoutSuccess: emptyProps(),
		LogoutFailure: props<{ error: any }>(),
		UpdateRefreshToken: props<{ refreshToken: string }>(),
		RefreshTokenUpdated: props<{ response: LoginUsuarioResponse }>(),
		RefreshTokenUpdateFailure: props<{ error: any }>(),
	},
});

export const setToken = createAction(
	'[Auth] Set Token', props<{ token: string, refreshToken: string }>()
)
