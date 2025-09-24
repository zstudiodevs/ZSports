import { Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RouterOutlet } from '@angular/router';
import { ShellComponent } from '@components/shell/shell.component';
import { LoginComponent } from './login/login.component';
import { Button } from '@components/buttons/button/button.component';
import { AuthService } from './services/auth.service';

const themeButton: Button = {
	id: 'theme-button',
	icon: 'contrast',
	disabled: false,
	htmlType: 'button',
	type: 'icon',
	color: undefined,
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
		color: undefined,
		label: '',
	},
];

const loggedOutButtons: Button[] = [
	themeButton,
	{
		id: 'login-button',
		label: 'Iniciar sesión',
		type: 'flat',
		color: 'primary',
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
	private dialog = inject(MatDialog);
	private readonly authService = inject(AuthService);

	protected secondaryButtons: Button[] = [];

	ngOnInit(): void {
		this.authService.token$.subscribe((isLoggedIn) => {
			this.secondaryButtons = isLoggedIn ? loggedInButtons : loggedOutButtons;
		});
	}

	onProfileButtonClicked() {
		const dialog = this.dialog.open(LoginComponent);
		dialog.afterClosed().subscribe((result) => {
			console.log('The dialog was closed');
		});
	}

	toggleTheme() {
		document.body.classList.toggle('dark-theme');
	}

	onSecondaryButtonClicked(buttonId: string) {
		switch (buttonId) {
			case 'login-button': {
				this.onProfileButtonClicked();
				break;
			}
			case 'theme-button': {
				this.toggleTheme();
				break;
			}
			default:
				break;
		}
		console.log('Secondary button clicked:', buttonId);
	}
}
