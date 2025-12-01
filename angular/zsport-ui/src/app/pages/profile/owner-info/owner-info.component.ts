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
	LoadingService,
	Button,
	ButtonComponent,
	SnackbarService,
} from '../../../shared';
import { MatCardModule } from '@angular/material/card';
import {
	Establecimiento,
	UpdateEstablecimiento,
} from './types/establecimiento.type';
import { EstablecimientoService } from './services/establecimiento.service';
import { EstablecimientoStore } from './store/establecimiento.store';
import { NavigationService } from '../../../services/navigation.service';
import { MatChipsModule } from '@angular/material/chips';
import { Subject, takeUntil } from 'rxjs';
import {
	FormControl,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { MatInputModule, MatLabel } from '@angular/material/input';

@Component({
	selector: 'zs-owner-info',
	templateUrl: './owner-info.component.html',
	styleUrl: './owner-info.component.scss',
	standalone: true,
	imports: [
		MatCardModule,
		ButtonComponent,
		MatChipsModule,
		FormsModule,
		ReactiveFormsModule,
		MatLabel,
		MatInputModule,
	],
})
export class OwnerInfoComponent implements OnInit {
	public loadingRef = input.required<ElementRef>();
	private readonly loadingService = inject(LoadingService);
	private readonly service = inject(EstablecimientoService);
	private readonly store = inject(EstablecimientoStore);
	private readonly navigationService = inject(NavigationService);
	private readonly snackbarService = inject(SnackbarService);
	private destroy$ = new Subject<void>();
	private isFormValid = signal<boolean>(false);

	public userId = input.required<string>();

	protected establecimiento!: Establecimiento;

	protected descripcionControl = new FormControl('', [
		Validators.required,
		Validators.maxLength(500),
	]);
	protected emailControl = new FormControl('', [
		Validators.required,
		Validators.email,
		Validators.maxLength(100),
	]);
	protected telefonoControl = new FormControl('', [
		Validators.required,
		Validators.pattern('^[0-9]+$'),
		Validators.maxLength(30),
	]);

	protected backButton: Button = {
		id: 'back',
		icon: 'arrow_back',
		buttonType: 'icon',
		htmlType: 'button',
		disabled: false,
		action: () => this.onBackButtonClicked(),
	};
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

	constructor() {
		effect(() => {});
	}

	ngOnInit(): void {
		this.loadingService.show(this.loadingRef()!);

		this.service.getByPropietario(this.userId()).subscribe({
			next: (establecimiento) => {
				this.establecimiento = establecimiento;
				this.store.setState({
					currentEstablecimiento: establecimiento,
					error: null,
				});
				this.descripcionControl.setValue(establecimiento.descripcion);
				this.emailControl.setValue(establecimiento.email);
				this.telefonoControl.setValue(establecimiento.telefono);
				this.loadingService.hide();
			},
			error: (error) => {
				this.store.setState({
					currentEstablecimiento: null,
					error: error.message,
				});
				this.loadingService.hide();
			},
		});
	}

	onBackButtonClicked() {
		this.navigationService.goBack();
	}

	onSaveButtonClicked() {
		if (this.isFormValid()) {
			const descripcion = this.descripcionControl.value!;
			const email = this.emailControl.value!;
			const telefono = this.telefonoControl.value!;

			const request: UpdateEstablecimiento = {
				id: this.establecimiento.id,
				descripcion,
				email,
				telefono,
			};
			this.loadingService.show(this.loadingRef()!);
			this.service
				.update(request)
				.pipe(takeUntil(this.destroy$))
				.subscribe({
					next: (establecimiento) => {
						this.establecimiento = establecimiento;
						this.store.setState({
							currentEstablecimiento: establecimiento,
							error: null,
						});
						this.loadingService.hide();
						this.snackbarService.open({
							message: 'Establecimiento actualizado correctamente',
							type: 'success',
							duration: 5000,
						});
					},
					error: (error) => {
						this.store.setState({
							currentEstablecimiento: null,
							error: error.message,
						});
						this.loadingService.hide();
						this.snackbarService.open({
							message: error.message
								? error.message
								: 'Error al actualizar el establecimiento',
							type: 'danger',
							duration: 5000,
						});
					},
				});
		}
	}

	onCancelButtonClicked() {
		if (this.establecimiento) {
			this.descripcionControl.setValue(this.establecimiento.descripcion);
			this.emailControl.setValue(this.establecimiento.email);
			this.telefonoControl.setValue(this.establecimiento.telefono);
		}
	}
}
