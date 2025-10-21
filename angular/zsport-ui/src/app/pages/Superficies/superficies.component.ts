import { Component, effect, inject } from '@angular/core';
import { SuperficiesService } from './services/superficies.service';
import { SuperficiesStore } from './store/superficies.store';
import { Superficie, superficiesColumns } from './types/superficies.types';
import { ToolbarComponent } from '@app/shared/toolbar/toolbar.component';
import { Button, ButtonComponent } from '@app/shared/buttons';
import { MatDialog } from '@angular/material/dialog';
import { CrearEditarSuperficieComponent } from './components/crear-editar-superficie/crear-editar-superficie.component';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { MatTableModule } from '@angular/material/table';
import { TitleCasePipe } from '@angular/common';
import { SnackbarService } from '@app/shared/snackbar/services/snackbar.service';

@Component({
	selector: 'zs-superficies',
	templateUrl: './superficies.component.html',
	styleUrl: './superficies.component.scss',
	standalone: true,
	imports: [ToolbarComponent, MatTableModule, TitleCasePipe, ButtonComponent],
})
export class SuperficiesComponent {
	private readonly superficiesService = inject(SuperficiesService);
	private readonly superficiesStore = inject(SuperficiesStore);
	private readonly snackbarService = inject(SnackbarService);
	private readonly dialog = inject(MatDialog);
	private readonly breakpointObserver = inject(BreakpointObserver);

	protected title = 'Superficies';
	private isMobile: boolean = false;
	protected backButton: Button = {
		id: 'back',
		icon: 'arrow_back',
		buttonType: 'icon',
		htmlType: 'button',
		disabled: false,
		action: () => window.history.back(),
	};
	protected toolbarActions: Button[] = [
		{
			id: 'add',
			label: 'Nueva',
			icon: 'add',
			buttonType: 'raised',
			htmlType: 'button',
			disabled: false,
			action: () => {
				this.dialog.open(CrearEditarSuperficieComponent, {
					data: { isEditMode: false, isMobile: this.isMobile },
					width: this.isMobile ? '100%' : '400px',
					maxWidth: '100%',
				});
			},
		},
	];

	protected displayedColumns: (keyof Superficie | 'actions')[] = [
		...superficiesColumns,
		'actions',
	];
	protected actions: Button[] = [
		{
			id: 'edit',
			icon: 'edit',
			buttonType: 'mini-fab',
			htmlType: 'button',
			disabled: false,
			color: 'info',
			action: () => {},
		},
		{
			id: 'disable',
			icon: 'block',
			buttonType: 'mini-fab',
			htmlType: 'button',
			disabled: false,
			color: 'danger',
			action: () => {},
		},
	];
	protected superficies: Superficie[] = [];

	constructor() {
		effect(() => {
			this.superficiesService.getAll().subscribe({
				next: (superficies) => {
					this.superficiesStore.setState({ superficies });
				},
				error: (error) => this.superficiesStore.setState({ error }),
			});
		});

		effect(() => {
			this.superficies = this.superficiesStore.superficies$();
		});

		effect(() => {
			this.breakpointObserver
				.observe(Breakpoints.XSmall)
				.subscribe((result) => {
					this.isMobile = result.matches;
				});
		});
	}

	protected onEdit(rowId: string) {
		this.superficiesService.getById(rowId).subscribe({
			next: (superficie) => {
				this.superficiesStore.setState({ currentSuperficie: superficie });
				this.dialog.open(CrearEditarSuperficieComponent, {
					data: {
						isEditMode: true,
						isMobile: this.isMobile,
						superficieId: rowId,
					},
					width: this.isMobile ? '100%' : '400px',
					maxWidth: '100%',
				});
			},
			error: (error) => {
				this.superficiesStore.setState({ error });
				this.snackbarService.open({
					message: 'Error al cargar la superficie.',
					duration: 3000,
					type: 'danger',
				});
			},
		});
	}
}
