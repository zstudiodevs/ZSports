import { Routes } from '@angular/router';
import { authGuard } from './auth/guard/auth.guard';

export const routes: Routes = [
	{
		path: '',
		children: [
			{
				path: 'my-profile',
				loadComponent: () =>
					import('./pages/profile/profile.component').then(
						(m) => m.ProfileComponent
					),
				canActivate: [authGuard],
			},
			{
				path: 'superficies',
				loadComponent: () =>
					import('./pages/Superficies/superficies.component').then(
						(m) => m.SuperficiesComponent
					),
				canActivate: [authGuard],
			},
			{
				path: 'canchas',
				loadComponent: () =>
					import('./pages/Canchas/canchas.component').then(
						(m) => m.CanchasComponent
					),
				canActivate: [authGuard],
			}
		],
	},
];
