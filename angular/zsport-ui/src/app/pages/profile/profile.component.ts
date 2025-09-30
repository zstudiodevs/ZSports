import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import {
	FormBuilder,
	FormControl,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule, MatLabel } from '@angular/material/input';
import { AuthService } from '@app/services/auth.service';
import { NavigationService } from '@app/services/navigation.service';
import { Button, ButtonComponent } from '@components/buttons';
import { UpdateUsuario, Usuario } from '@shared/usuarios';
import { filter, Observable, Subject, takeUntil } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { authActions } from '../../../state/auth/auth.actions';

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
export class ProfileComponent implements OnInit, OnDestroy {
	private readonly navigationService = inject(NavigationService);
	private readonly authService = inject(AuthService);
	private readonly formBuilder = inject(FormBuilder);
	private destroy$ = new Subject<void>();
	private formInitialData: Usuario;

	protected loading$: Observable<boolean>;
	protected username$: Observable<string | null>;
	protected usuario$: Observable<Usuario | null>;

	protected form: FormGroup = this.formBuilder.nonNullable.group({
		nombre: new FormControl<string>('', [
			Validators.required,
			Validators.maxLength(100),
		]),
		apellido: new FormControl<string>('', [
			Validators.required,
			Validators.maxLength(100),
		]),
		email: new FormControl<string>('', [
			Validators.required,
			Validators.email,
			Validators.maxLength(200),
		]),
	});

	protected backButton: Button = {
		id: 'back-button',
		icon: 'arrow_back',
		disabled: false,
		htmlType: 'button',
		type: 'icon',
		label: '',
	};

	protected saveButton: Button = {
		id: 'save-button',
		icon: 'save',
		label: 'Guardar',
		disabled: this.form.invalid,
		htmlType: 'submit',
		type: 'raised',
	};

	protected cancelButton: Button = {
		id: 'cancel-button',
		icon: 'cancel',
		label: 'Cancelar',
		disabled: false,
		htmlType: 'button',
		type: 'stroked',
	};

	ngOnInit(): void {
		this.loading$ = this.authService.loading$.pipe(
			filter((x) => !!x),
			takeUntil(this.destroy$)
		);
		this.username$ = this.authService.username$.pipe(
			filter((x) => !!x),
			takeUntil(this.destroy$)
		);
		this.usuario$ = this.authService.usuario$.pipe(
			filter((x) => !!x),
			takeUntil(this.destroy$)
		);

		this.usuario$
			.pipe(
				filter((usuario) => !!usuario),
				takeUntil(this.destroy$)
			)
			.subscribe((usuario) => {
				if (usuario) {
					this.formInitialData = usuario;
					this.form.patchValue({
						nombre: usuario.nombre,
						apellido: usuario.apellido,
						email: usuario.email,
					});
				}
			});
	}

	ngOnDestroy(): void {
		this.destroy$.next();
		this.destroy$.complete();
	}

	onBackButtonClicked() {
		this.navigationService.goBack();
	}

	onSaveButtonClicked() {
		// Lógica para guardar los cambios del perfil
		if (this.form.valid) {
			const updatedProfile = this.form.value;
			const request: UpdateUsuario = {
				id: this.formInitialData.id,
				nombre: updatedProfile.nombre,
				apellido: updatedProfile.apellido,
				email: updatedProfile.email,
			};

			this.authService.dispatch(
				authActions.updateUsuario({
					data: request,
				})
			);

			this.authService.updateSucceded$
				.pipe(
					filter((result) => result !== null),
					takeUntil(this.destroy$)
				)
				.subscribe((result) => {
					if (result.succeded) {
						// Actualización exitosa, manejar según sea necesario
						console.log('Perfil actualizado con éxito');
					} else {
						// Manejar errores de actualización
						console.error('Error al actualizar el perfil:', result.errors);
					}
				});
		}
	}

	onCancelButtonClicked() {
		// Lógica para cancelar los cambios y posiblemente restaurar los valores originales
		if (this.formInitialData) {
			this.form.reset();
			this.form.patchValue({
				nombre: this.formInitialData.nombre,
				apellido: this.formInitialData.apellido,
				email: this.formInitialData.email,
			});
		}
	}
}
