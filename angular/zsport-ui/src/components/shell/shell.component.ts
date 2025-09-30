import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, output } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { BreakpointObserver } from '@angular/cdk/layout';
import {
	Button,
	ButtonComponent,
	DropdownButton,
	DropdownButtonComponent,
} from '@components/buttons';
import { AuthService } from '@app/services/auth.service';

@Component({
	selector: 'zs-shell',
	templateUrl: './shell.component.html',
	styleUrl: './shell.component.scss',
	imports: [
		CommonModule,
		MatToolbarModule,
		MatSidenavModule,
		ButtonComponent,
		DropdownButtonComponent,
	],
})
export class ShellComponent implements OnInit {
	sidenavMode: 'side' | 'over' = 'side';
	isSidenavOpened = true;
	isDarkTheme = true;

	protected profileDropdownButton: DropdownButton = {
		id: 'profile-menu',
		icon: 'account_circle',
		buttonType: 'icon',
		htmlType: 'button',
		disabled: false,
		items: [],
	};
	@Input() secondaryButtons: Button[] = [];
	public secondaryButtonClicked = output<string>();

	constructor(
		private breakpointObserver: BreakpointObserver,
		private authService: AuthService
	) {}

	ngOnInit() {
		this.breakpointObserver
			.observe(['(max-width: 600px)'])
			.subscribe((result) => {
				this.sidenavMode = result.matches ? 'over' : 'side';
				this.isSidenavOpened = !result.matches;
			});

		this.authService.loggedInSucceded$.subscribe((response) => {
			if (response && response.succeded) {
				this.profileDropdownButton.items = [
					{
						id: 'profile',
						type: 'simple',
						disabled: false,
						htmlType: 'button',
						label: 'Perfil',
						icon: 'person',
					},
					{
						id: 'logout',
						type: 'simple',
						disabled: false,
						htmlType: 'button',
						label: 'Cerrar Sesión',
						icon: 'logout',
					},
				];
			} else {
				this.profileDropdownButton.items = [
					{
						id: 'login',
						type: 'simple',
						disabled: false,
						htmlType: 'button',
						label: 'Iniciar Sesión',
						icon: 'login',
					},
				];
			}
		});
	}
}
