import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
	{
		path: '',
		children: [
			{
				path: 'my-profile',
				loadComponent: () => import('./pages/profile/profile.component').then(m => m.ProfileComponent),
				canActivate: [AuthGuard],
			},
		],
	},
];
