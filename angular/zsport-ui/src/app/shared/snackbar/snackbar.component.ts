import { Component, inject, Inject, signal } from '@angular/core';
import {
	MAT_SNACK_BAR_DATA,
	MatSnackBarAction,
	MatSnackBarActions,
	MatSnackBarLabel,
	MatSnackBarRef,
} from '@angular/material/snack-bar';
import { SnackbarConfig } from './types/snackbar.types';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
	selector: 'zs-snackbar',
	templateUrl: './snackbar.component.html',
	styleUrls: ['./snackbar.component.scss'],
	standalone: true,
	imports: [
		MatSnackBarLabel,
		MatSnackBarActions,
		MatSnackBarAction,
		MatIconModule,
		MatButtonModule,
	],
})
export class SnackbarComponent {
	protected readonly snackbarRef = inject(MatSnackBarRef);
	protected message = signal<string>('Mensaje de relleno');

	constructor(@Inject(MAT_SNACK_BAR_DATA) public data: SnackbarConfig) {
		if (data && data.message) {
			this.message.set(data.message);
		}
	}
}
