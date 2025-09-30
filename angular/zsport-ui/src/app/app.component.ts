import { Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RouterOutlet } from '@angular/router';
import { ShellComponent } from '@components/shell/shell.component';
import { LoginComponent } from './login/login.component';
import { Button } from '@components/buttons';
import { AuthService } from './services/auth.service';
import { NavigationService } from './services/navigation.service';
import { Subject, take, takeUntil } from 'rxjs';
import { authActions } from '../state/auth/auth.actions';

const themeButton: Button = {
	id: 'theme-button',
	icon: 'contrast',
	disabled: false,
	htmlType: 'button',
	type: 'icon',
	label: '',
};

const loggedInButtons: Button[] = [
	themeButton,
	{
		id: 'profile-button',
		icon: 'account_circle',
		disabled: false,
		htmlType: 'button',
		type: 'icon',
		label: '',
	},
];

const loggedOutButtons: Button[] = [
	themeButton,
	{
		id: 'login-button',
		label: 'Iniciar sesión',
		type: 'flat',
		disabled: false,
		htmlType: 'button',
	},
];

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrl: './app.component.scss',
	standalone: true,
	imports: [RouterOutlet, ShellComponent],
})
export class AppComponent implements OnInit {
	title = 'zsport-ui';
	private destroy$ = new Subject<void>();
	private dialog = inject(MatDialog);
	private readonly authService = inject(AuthService);
	private readonly navigationService = inject(NavigationService);

	protected secondaryButtons: Button[] = [];

	ngOnInit(): void {
		this.authService.token$
			.pipe(takeUntil(this.destroy$))
			.subscribe((isLoggedIn) => {
				this.secondaryButtons = isLoggedIn ? loggedInButtons : loggedOutButtons;
			});
	}

	onLoginButtonClicked() {
		const dialog = this.dialog.open(LoginComponent);
		dialog.afterClosed().subscribe((result) => {
			console.log('The dialog was closed');
		});
	}

	onLogoutButtonClicked() {
		this.authService.refreshToken$.pipe(take(1)).subscribe((refreshToken) => {
			if (refreshToken) {
				this.authService.dispatch(authActions.logout({ refreshToken }));
			}
		});

		this.navigationService.navigateTo(['/']);
	}

	toggleTheme() {
		document.body.classList.toggle('dark-theme');
	}

	onSecondaryButtonClicked(buttonId: string) {
		switch (buttonId) {
			case 'login': {
				this.onLoginButtonClicked();
				break;
			}
			case 'theme-button': {
				this.toggleTheme();
				break;
			}
			case 'profile': {
				this.navigationService.navigateTo(['/my-profile']);
				break;
			}
			case 'logout': {
				this.onLogoutButtonClicked();
				break;
			}
			default:
				break;
		}
		console.log('Secondary button clicked:', buttonId);
	}
}
