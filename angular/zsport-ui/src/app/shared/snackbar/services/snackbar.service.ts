import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackbarComponent } from '../snackbar.component';
import { SnackbarConfig } from '../types/snackbar.types';

@Injectable({
	providedIn: 'root',
})
export class SnackbarService {
	private snackbar = inject(MatSnackBar);
	private defaultDuration = 3000;

	public open(config: SnackbarConfig) {
		this.snackbar.openFromComponent(SnackbarComponent, {
			data: { message: config.message },
			duration: config.duration ?? this.defaultDuration,
			horizontalPosition: 'center',
			verticalPosition: 'bottom',
			panelClass: ['snackbar', config.type ?? 'info'],
		});
	}
}
