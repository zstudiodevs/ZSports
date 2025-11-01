import { Component, effect, ElementRef, inject, input } from '@angular/core';
import { LoadingService } from '@app/shared/loading/services/loading.service';
import { MatCardModule } from '@angular/material/card';
import { Establecimiento } from './types/establecimiento.type';
import { EstablecimientoService } from './services/establecimiento.service';
import { EstablecimientoStore } from './store/establecimiento.store';

@Component({
	selector: 'zs-owner-info',
	templateUrl: './owner-info.component.html',
	styleUrl: './owner-info.component.scss',
	standalone: true,
	imports: [MatCardModule],
})
export class OwnerInfoComponent {
	public loadingRef = input.required<ElementRef>();
	private readonly loadingService = inject(LoadingService);
	private readonly service = inject(EstablecimientoService);
	private readonly store = inject(EstablecimientoStore);

	protected establecimiento: Establecimiento;

	constructor() {
		effect(() => {});
	}
}
