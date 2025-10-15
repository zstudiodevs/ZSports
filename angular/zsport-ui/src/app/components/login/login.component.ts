import {
	Component,
	inject,
} from '@angular/core';
import {
	FormControl,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule, MatLabel } from '@angular/material/input';
import { AuthStore } from '@app/auth/store/auth.store';
import { AuthService } from '@app/auth/auth.service';
import { SnackbarService } from '@app/shared/snackbar/services/snackbar.service';
import { MatDialogRef } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { LoadingService } from '@app/shared/loading/services/loading.service';

@Component({
	selector: 'zs-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
	standalone: true,
	imports: [
		MatCardModule,
		MatFormFieldModule,
		MatInputModule,
		MatLabel,
		MatButtonModule,
		MatIconModule,
		FormsModule,
		ReactiveFormsModule,
	],
})
export class LoginComponent {
	private dialogRef = inject(MatDialogRef<LoginComponent>);
	private authService = inject(AuthService);
	private loadingService = inject(LoadingService);
	private snackbarService = inject(SnackbarService);
	private authStore = inject(AuthStore);

	protected emailControl = new FormControl('', [
		Validators.required,
		Validators.email,
		Validators.maxLength(256),
	]);

	protected passwordControl = new FormControl('', [
		Validators.required,
		Validators.minLength(6),
	]);

	protected onCloseDialog = () => this.dialogRef.close();

	protected onLogin() {
		if (this.emailControl.valid && this.passwordControl.valid) {
			const email = this.emailControl.value;
			const password = this.passwordControl.value;
			this.loadingService.show(this.dialogRef.componentRef?.location!);
			if (email && password) {
				this.authService.login({ email, password }).subscribe({
					next: (response) => {
						this.authStore.setState({
							token: response.token,
							refreshToken: response.refreshToken,
							username: response.username,
							user: response.usuario,
							isAuthenticated: true,
						});

						this.snackbarService.open({
							message: `${response.usuario.nombre}, bienvenido a ZSports!`,
							duration: 5000,
							type: 'success',
						});

						this.loadingService.hide();
						this.dialogRef.close(true);
					},
					error: (error) => {
						this.loadingService.hide();
						this.authStore.setState({
							token: null,
							refreshToken: null,
							username: null,
							user: null,
							isAuthenticated: false,
						});
						this.snackbarService.open({
							message: error.error?.message || 'Error en el inicio de sesión',
							duration: 5000,
							type: 'danger',
						});
					},
				});
			}
		} else {
			this.snackbarService.open({
				message: 'Por favor, complete el formulario correctamente.',
				duration: 5000,
				type: 'warning',
			});
		}
	}
}
