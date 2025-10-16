import { Component, effect, Inject, inject } from '@angular/core';
import { SuperficiesService } from '../../services/superficies.service';
import { SuperficiesStore } from '../../store/superficies.store';
import {
	FormControl,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatFormField, MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SnackbarService } from '@app/shared/snackbar/services/snackbar.service';
import { LoadingService } from '@app/shared/loading/services/loading.service';

export interface CrearEditarSuperficieData {
	isEditMode: boolean;
	isMobile?: boolean;
	superficieId?: string;
}

@Component({
	selector: 'zs-crear-editar-superficie',
	templateUrl: './crear-editar-superficie.component.html',
	styleUrl: './crear-editar-superficie.component.scss',
	standalone: true,
	imports: [
		MatCardModule,
		FormsModule,
		ReactiveFormsModule,
		MatFormField,
		MatInputModule,
		MatIconModule,
		MatButtonModule,
	],
})
export class CrearEditarSuperficieComponent {
	private dialogRef = inject(MatDialogRef<CrearEditarSuperficieComponent>);
	private service = inject(SuperficiesService);
	private store = inject(SuperficiesStore);
	private snackbarService = inject(SnackbarService);
	private loadingService = inject(LoadingService);

	protected idControl = new FormControl<string>('', [Validators.required]);
	protected nombreControl = new FormControl<string>('', [
		Validators.required,
		Validators.maxLength(64),
	]);
	protected isEditMode: boolean = false;
	protected isMobile?: boolean = false;

	constructor(
		@Inject(MAT_DIALOG_DATA) private data: CrearEditarSuperficieData
	) {
		this.isEditMode = data.isEditMode;
		this.isMobile = data.isMobile;

		effect(() => {
			if (this.isEditMode) {
				this.loadingService.show(this.dialogRef.componentRef?.location!);
				let superficie = this.store.currentSuperficie$();
				if (superficie) {
					this.idControl.setValue(superficie.id);
					this.nombreControl.setValue(superficie.nombre);
					this.loadingService.hide();
				}
			}
		});
	}

	protected onCloseDialog = () => this.dialogRef.close();
	protected onSave() {
		this.loadingService.show(this.dialogRef.componentRef?.location!);
		if (this.isEditMode) {
			if (this.idControl.invalid || this.nombreControl.invalid) {
				this.loadingService.hide();
				this.snackbarService.open({
					message: 'Por favor, complete todos los campos requeridos.',
					duration: 3000,
				});
				return;
			}

			const id = this.idControl.value;
			const nombre = this.nombreControl.value;

			if (!id || !nombre) {
				this.loadingService.hide();
				this.snackbarService.open({
					message: 'Por favor, complete todos los campos requeridos.',
					duration: 3000,
				});
				return;
			}
			// TODO: Implementar edición de superficie
			this.loadingService.hide();
			this.snackbarService.open({
				message: 'Funcionalidad de edición no implementada aún.',
				duration: 3000,
			});
			this.dialogRef.close();
		} else {
			if (this.nombreControl.invalid) {
				this.loadingService.hide();
				this.snackbarService.open({
					message: 'Por favor, complete todos los campos requeridos.',
					duration: 3000,
				});
				return;
			}
			const nombre = this.nombreControl.value;
			if (!nombre) {
				this.loadingService.hide();
				this.snackbarService.open({
					message: 'Por favor, complete todos los campos requeridos.',
					duration: 3000,
				});
				return;
			}
			this.service.create(nombre).subscribe({
				next: (superficie) => {
					this.store.addSuperficie(superficie);
					this.loadingService.hide();
					this.snackbarService.open({
						message: 'Superficie creada con éxito.',
						duration: 3000,
						type: 'success',
					});
					this.dialogRef.close();
				},
				error: (error) => {
					this.store.setState({
						error: error.error?.message || 'Error al crear la superficie.',
					});
					this.loadingService.hide();
					this.snackbarService.open({
						message: error.error?.message || 'Error al crear la superficie.',
						duration: 3000,
						type: 'danger',
					});
				},
			});
		}
	}
}
