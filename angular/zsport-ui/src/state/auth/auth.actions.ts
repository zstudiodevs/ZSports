import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { LoginUsuarioResponse } from '../../shared/usuarios';

export const authActions = createActionGroup({
	source: 'Auth',
	events: {
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
