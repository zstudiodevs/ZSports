import {
	Component,
	effect,
	ElementRef,
	inject,
	OnInit,
	signal,
	ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { Button, DropdownButton } from '@components/buttons';
import { NavigationService } from './services/navigation.service';
import { ToolbarComponent } from './shared/toolbar/toolbar.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SidebarService } from './shared/sidebar/services/sidebar.service';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { SidebarComponent } from './shared/sidebar/sidebar.component';
import { AuthStore } from './auth/store/auth.store';
import { AuthService } from './auth/auth.service';
import { LoadingService } from './shared/loading/services/loading.service';
import { SnackbarService } from './shared/snackbar/services/snackbar.service';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrl: './app.component.scss',
	standalone: true,
	imports: [RouterOutlet, ToolbarComponent, MatSidenavModule, SidebarComponent],
})
export class AppComponent implements OnInit {
	private readonly sidebarService = inject(SidebarService);
	private readonly snackbarService = inject(SnackbarService);
	private readonly navigationService = inject(NavigationService);
	private readonly loadingService = inject(LoadingService);
	private readonly authService = inject(AuthService);
	private readonly authStore = inject(AuthStore);
	private readonly dialog = inject(MatDialog);
	private readonly breakpointObserver = inject(BreakpointObserver);

	title = 'ZSports';
	@ViewChild('loadingRef', { static: true }) loadingRef: ElementRef;
	protected isSidebarOpened = this.sidebarService.isSidebarOpened;
	protected sidebarMode = this.sidebarService.getSidebarMode;
	protected mainButton: Button = {
		id: 'menu',
		icon: 'menu',
		type: 'icon',
		htmlType: 'button',
		disabled: false,
		action: () => this.onMenuButtonClicked(),
	};
	protected profileButton = signal<DropdownButton>({
		id: 'profile',
		label: '',
		icon: 'account_circle',
		buttonType: 'flat',
		htmlType: 'button',
		disabled: false,
		items: [],
	});

	protected secondaryButtons = signal<(Button | DropdownButton)[]>([
		{
			id: 'theme',
			icon: 'contrast',
			type: 'icon',
			htmlType: 'button',
			disabled: false,
			action: () => this.toggleTheme(),
		},
		this.profileButton(),
	]);

	constructor() {
		effect(() => {
			this.loadingService.show(this.loadingRef);
			let user = this.authStore.user$();
			if (user) {
				this.profileButton.update((currentState) => ({
					...currentState,
					label: `${user.nombre} ${user.apellido}`,
					items: [
						{
							id: 'profile',
							label: 'Mi Perfil',
							icon: 'person',
							type: 'flat',
							htmlType: 'button',
							disabled: false,
							action: () => this.onSecondaryButtonClicked('profile'),
						},
						{
							id: 'logout',
							label: 'Cerrar Sesión',
							icon: 'logout',
							type: 'flat',
							htmlType: 'button',
							disabled: false,
							action: () => this.onSecondaryButtonClicked('logout'),
						},
					],
				}));
				this.secondaryButtons.update((currentState) => {
					const filtered = currentState.filter((btn) => btn.id !== 'profile');
					return [...filtered, this.profileButton()];
				});
			} else {
				this.profileButton.update((currentState) => ({
					...currentState,
					label: 'Iniciar Sesión',
					items: [
						{
							id: 'login',
							label: 'Iniciar Sesión',
							icon: 'login',
							type: 'flat',
							htmlType: 'button',
							disabled: false,
							action: () => this.onSecondaryButtonClicked('login'),
						},
					],
				}));

				this.secondaryButtons.update((currentState) => {
					const filtered = currentState.filter((btn) => btn.id !== 'profile');
					return [...filtered, this.profileButton()];
				});
			}
			this.loadingService.hide();
		});
	}

	ngOnInit(): void {
		this.breakpointObserver.observe(Breakpoints.XSmall).subscribe((result) => {
			const isMobile = result.matches;
		});
	}

	onMenuButtonClicked() {
		this.sidebarService.toggleSidebar();
	}

	onSidebarBackdropClicked() {
		this.sidebarService.closeSidebar();
	}

	onLogoutButtonClicked() {
		const refreshToken = this.authStore.refreshToken$();
		this.loadingService.show(this.loadingRef);
		if (refreshToken)
			this.authService.logout(refreshToken).subscribe({
				next: () => {
					this.snackbarService.open({
						message: 'Sesión cerrada con éxito',
						duration: 5000,
						type: 'success',
					});
					this.loadingService.hide();
					this.authStore.resetState();
					this.navigationService.navigateTo(['/']);
				},
				error: (error) => {
					this.snackbarService.open({
						message: error.error?.message || 'Error al cerrar sesión',
						duration: 5000,
						type: 'danger',
					});
					this.loadingService.hide();
				},
			});
	}

	toggleTheme() {
		document.body.classList.toggle('dark-theme');
	}

	onSecondaryButtonClicked(buttonId: string) {
		switch (buttonId) {
			case 'login': {
				this.dialog.open(LoginComponent);
				break;
			}
			case 'theme': {
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
