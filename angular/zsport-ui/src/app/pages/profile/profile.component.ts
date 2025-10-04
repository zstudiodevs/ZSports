import { CommonModule } from '@angular/common';
import {
	Component,
	computed,
	effect,
	ElementRef,
	inject,
	signal,
	ViewChild,
} from '@angular/core';
import {
	FormControl,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule, MatLabel } from '@angular/material/input';
import { NavigationService } from '@app/services/navigation.service';
import { Button, ButtonComponent } from '@components/buttons';
import { Subject, takeUntil } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoadingService } from '@app/shared/loading/services/loading.service';
import { AuthStore } from '@app/auth/store/auth.store';
import { UpdateUserRequest, User } from '@app/auth/types/auth.type';
import { SnackbarService } from '@app/shared/snackbar/services/snackbar.service';
import { AuthService } from '@app/auth/auth.service';

@Component({
	selector: 'zs-profile',
	templateUrl: './profile.component.html',
	styleUrl: './profile.component.scss',
	standalone: true,
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		MatCardModule,
		MatInputModule,
		MatIconModule,
		MatLabel,
		ButtonComponent,
		MatProgressSpinnerModule,
	],
})
export class ProfileComponent {
	@ViewChild('loadingRef', { static: true }) loadingRef: ElementRef;
	private readonly loadingService = inject(LoadingService);
	private readonly navigationService = inject(NavigationService);
	private readonly snackbarService = inject(SnackbarService);
	private readonly authService = inject(AuthService);
	private readonly authStore = inject(AuthStore);
	private destroy$ = new Subject<void>();
	private isFormValid = signal<boolean>(false);

	protected username: string | null = null;
	protected user: User | null = null;

	protected nombreControl = new FormControl('', [Validators.required]);
	protected apellidoControl = new FormControl('', [Validators.required]);
	protected emailControl = new FormControl('', [
		Validators.required,
		Validators.email,
	]);

	protected saveButton = computed<Button>(() => ({
		id: 'save',
		label: 'Guardar',
		icon: 'save',
		type: 'raised',
		htmlType: 'submit',
		disabled: !this.isFormValid(),
		action: () => this.onSaveButtonClicked(),
	}));
	protected cancelButton = computed<Button>(() => ({
		id: 'cancel',
		label: 'Cancelar',
		icon: 'cancel',
		type: 'stroked',
		htmlType: 'button',
		disabled: false,
		action: () => this.onCancelButtonClicked(),
	}));
	protected backButton: Button = {
		id: 'back',
		icon: 'arrow_back',
		type: 'icon',
		htmlType: 'button',
		disabled: false,
		action: () => this.onBackButtonClicked(),
	};

	constructor() {
		effect(() => {
			this.loadingService.show(this.loadingRef);
			const user = this.authStore.user$();
			if (user) {
				this.user = user;
				this.nombreControl.setValue(user.nombre);
				this.apellidoControl.setValue(user.apellido);
				this.emailControl.setValue(user.email);
				this.loadingService.hide();
			} else {
				this.snackbarService.open({
					message: 'Error al cargar los datos del usuario',
					type: 'danger',
					duration: 5000,
				});
			}
		});
		effect(() => {
			this.username = this.authStore.username$();
		});

		this.nombreControl.valueChanges
			.pipe(takeUntil(this.destroy$))
			.subscribe(() => this.updateFormValidity());
		this.apellidoControl.valueChanges
			.pipe(takeUntil(this.destroy$))
			.subscribe(() => this.updateFormValidity());
		this.emailControl.valueChanges
			.pipe(takeUntil(this.destroy$))
			.subscribe(() => this.updateFormValidity());
	}

	private updateFormValidity() {
		this.isFormValid.set(
			this.nombreControl.valid &&
				this.apellidoControl.valid &&
				this.emailControl.valid
		);
	}

	onBackButtonClicked() {
		this.navigationService.goBack();
	}

	onSaveButtonClicked() {
		if (this.isFormValid()) {
			const nombre = this.nombreControl.value!;
			const apellido = this.apellidoControl.value!;
			const email = this.emailControl.value!;
			const request: UpdateUserRequest = {
				id: this.user!.id,
				nombre,
				apellido,
				email,
			};
			this.loadingService.show(this.loadingRef);
			this.authService
				.updateProfile(request)
				.pipe(takeUntil(this.destroy$))
				.subscribe({
					next: (user) => {
						this.authStore.setState({ user });
						this.loadingService.hide();
						this.snackbarService.open({
							message: 'Perfil actualizado correctamente',
							type: 'success',
						});
					},
					error: (error) => {
						this.loadingService.hide();
						this.snackbarService.open({
							message: error.message
								? error.message
								: 'Error al actualizar el perfil',
							type: 'danger',
							duration: 5000,
						});
					},
				});
		}
	}

	onCancelButtonClicked() {
		if (this.user) {
			this.nombreControl.setValue(this.user.nombre);
			this.apellidoControl.setValue(this.user.apellido);
			this.emailControl.setValue(this.user.email);
		}
	}
}
