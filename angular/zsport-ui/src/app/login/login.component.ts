
import { Component, inject, OnInit } from '@angular/core';
import {
	FormBuilder,
	FormControl,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule, MatLabel } from '@angular/material/input';
import { AuthService } from '@app/services/auth.service';
import { authActions } from '../../state/auth/auth.actions';

@Component({
	selector: 'zs-login',
	templateUrl: './login.component.html',
	styleUrl: './login.component.scss',
	standalone: true,
	imports: [
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatInputModule,
    MatIconModule,
    MatLabel,
    MatButtonModule
],
})
export class LoginComponent {
	private readonly dialogRef = inject(MatDialogRef<LoginComponent>);
	private readonly authService = inject(AuthService);
	private readonly formBuilder = inject(FormBuilder);
	protected isRegistering: boolean = false;
	protected formGroup: FormGroup = this.formBuilder.nonNullable.group({
		email: new FormControl<string>('', [
			Validators.required,
			Validators.email,
			Validators.maxLength(255),
		]),
		password: new FormControl<string>('', [
			Validators.required,
			Validators.maxLength(255),
		]),
	});

	public onRegisterTextClick() {
		this.isRegistering = !this.isRegistering;
		this.formGroup.reset();
		if (this.isRegistering) {
			this.formGroup.addControl('userName', new FormControl<string>('', [
			Validators.required,
			Validators.maxLength(255),
		]));
		} else {
			this.formGroup.removeControl('userName');
		}
	}

	public onLogin(): void {
		if (this.formGroup.valid) {
			const { email, password } = this.formGroup.getRawValue();
			this.authService.dispatch(
				authActions.login({
					email: email,
					password: password,
				})
			);

			this.authService.loggedInSucceded$
			.subscribe((response) => {
				if (response.succeded) {
					this.dialogRef.close();
				} else {
					this.formGroup.setErrors({ loginError: response.errors });
				}
			});
		}
	}

	public onRegister(): void {
		if (this.formGroup.valid) {
			const { email, password, userName } = this.formGroup.getRawValue();
			this.authService.dispatch(
				authActions.register({
					email: email,
					password: password,
					userName: userName,
				})
			);

			this.authService.registerSucceded$
			.subscribe((response) => {
				if (response.succeded) {
					this.onRegisterTextClick();
				} else {
					this.formGroup.setErrors({ registerError: response.errors });
				}
			});
		}
	}

	public onCancel(): void {
		this.dialogRef.close();
	}
}
