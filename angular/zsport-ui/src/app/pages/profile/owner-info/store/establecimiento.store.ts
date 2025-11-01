import { Injectable, signal } from '@angular/core';
import {
	Establecimiento,
	EstablecimientoState,
} from '../types/establecimiento.type';

@Injectable({
	providedIn: 'root',
})
export class EstablecimientoStore {
	private readonly initialState: EstablecimientoState = {
		currentEstablecimiento: null,
		error: null,
	};

	private state = signal<EstablecimientoState>(this.initialState);
	public readonly state$ = this.state.asReadonly();

	private currentEstablecimiento = signal<Establecimiento | null>(
		this.initialState.currentEstablecimiento
	);
	public readonly currentEstablecimiento$ =
		this.currentEstablecimiento.asReadonly();

	private error = signal<string | null>(this.initialState.error);
	public readonly error$ = this.error.asReadonly();

	public setState(newState: Partial<EstablecimientoState>) {
		if (newState.currentEstablecimiento !== undefined) {
			this.currentEstablecimiento.set(newState.currentEstablecimiento);
		}

		if (newState.error !== undefined) {
			this.error.set(newState.error);
		}

		this.state.update((currentState) => ({ ...currentState, ...newState }));
	}

	public resetState() {
		this.state.set(this.initialState);
		this.currentEstablecimiento.set(this.initialState.currentEstablecimiento);
		this.error.set(this.initialState.error);
	}

	public updateEstablecimiento(establecimiento: Establecimiento) {
		this.currentEstablecimiento.set(establecimiento);
	}
}
