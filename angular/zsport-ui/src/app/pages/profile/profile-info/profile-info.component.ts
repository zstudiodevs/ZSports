import { CommonModule } from '@angular/common';
import {
	Component,
	computed,
	effect,
	ElementRef,
	inject,
	input,
	OnInit,
	signal,
} from '@angular/core';
import {
	FormControl,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule, MatLabel } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '@app/auth/auth.service';
import { AuthStore } from '@app/auth/store/auth.store';
import { User, UpdateUserRequest, UserRoles } from '@app/auth/types/auth.type';
import { NavigationService } from '@app/services/navigation.service';
import { Button, ButtonComponent } from '@app/shared/buttons';
import { LoadingService } from '@app/shared/loading/services/loading.service';
import { SnackbarService } from '@app/shared/snackbar/services/snackbar.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
	selector: 'zs-profile-info',
	templateUrl: './profile-info.component.html',
	styleUrls: ['./profile-info.component.scss'],
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
		MatChipsModule,
	],
})
export class ProfileInfoComponent implements OnInit {
	public loadingRef = input<ElementRef>();
	public user = input.required<User>();
	public username = input.required<string>();
	private readonly loadingService = inject(LoadingService);
	private readonly navigationService = inject(NavigationService);
	private readonly snackbarService = inject(SnackbarService);
	private readonly authService = inject(AuthService);
	private readonly authStore = inject(AuthStore);
	private destroy$ = new Subject<void>();
	private isFormValid = signal<boolean>(false);

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
		buttonType: 'raised',
		htmlType: 'submit',
		disabled: !this.isFormValid(),
		action: () => this.onSaveButtonClicked(),
	}));
	protected cancelButton = computed<Button>(() => ({
		id: 'cancel',
		label: 'Cancelar',
		icon: 'cancel',
		buttonType: 'stroked',
		htmlType: 'button',
		disabled: false,
		action: () => this.onCancelButtonClicked(),
	}));
	protected backButton: Button = {
		id: 'back',
		icon: 'arrow_back',
		buttonType: 'icon',
		htmlType: 'button',
		disabled: false,
		action: () => this.onBackButtonClicked(),
	};
	protected ownerButton: Button = {
		id: 'owner',
		label: 'Propietario',
		icon: 'badge',
		buttonType: 'flat',
		htmlType: 'button',
		disabled: false,
		action: () => {},
	};

	constructor() {}

	ngOnInit(): void {
		this.loadingService.show(this.loadingRef()!);
		this.nombreControl.setValue(this.user().nombre);
		this.apellidoControl.setValue(this.user().apellido);
		this.emailControl.setValue(this.user().email);

		this.nombreControl.valueChanges
			.pipe(takeUntil(this.destroy$))
			.subscribe(() => this.updateFormValidity());
		this.apellidoControl.valueChanges
			.pipe(takeUntil(this.destroy$))
			.subscribe(() => this.updateFormValidity());
		this.emailControl.valueChanges
			.pipe(takeUntil(this.destroy$))
			.subscribe(() => this.updateFormValidity());
		this.loadingService.hide();
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
				id: this.user().id,
				nombre,
				apellido,
				email,
			};
			this.loadingService.show(this.loadingRef()!);
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
		if (this.user()) {
			this.nombreControl.setValue(this.user().nombre);
			this.apellidoControl.setValue(this.user().apellido);
			this.emailControl.setValue(this.user().email);
		}
	}

	isOwner(): boolean {
		return this.user().roles.includes(UserRoles.Owner) || false;
	}
}
