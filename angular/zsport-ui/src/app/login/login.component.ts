import { CommonModule } from '@angular/common';
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
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
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
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		MatCardModule,
		MatInputModule,
		MatIconModule,
		MatLabel,
		MatButtonModule,
	],
})
export class LoginComponent {
	private readonly dialogRef = inject(MatDialogRef<LoginComponent>);
	private readonly authService = inject(AuthService);
	private readonly formBuilder = inject(FormBuilder);
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

	public onLogin(): void {
		if (this.formGroup.valid) {
			const { email, password } = this.formGroup.getRawValue();
			this.authService.dispatch(
				authActions.login({
					email: email,
					password: password,
				})
			);
		}
	}

	public onCancel(): void {
		this.dialogRef.close();
	}
}
