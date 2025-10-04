import { Routes } from '@angular/router';

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
				canActivate: [],
			},
		],
	},
];
