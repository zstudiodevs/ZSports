import { Injectable, signal } from '@angular/core';
import { Superficie, SuperficiesState } from '../types/superficies.types';

@Injectable({
	providedIn: 'root',
})
export class SuperficiesStore {
	private readonly initialState: SuperficiesState = {
		superficies: [],
		currentSuperficie: null,
		error: null,
	};

	private state = signal<SuperficiesState>(this.initialState);
	public readonly state$ = this.state.asReadonly();

	private superficies = signal<Superficie[]>(this.initialState.superficies);
	public readonly superficies$ = this.superficies.asReadonly();

	private currentSuperficie = signal<Superficie | null>(
		this.initialState.currentSuperficie
	);
	public readonly currentSuperficie$ = this.currentSuperficie.asReadonly();

	private error = signal<string | null>(this.initialState.error);
	public readonly error$ = this.error.asReadonly();

	public setState(newState: Partial<SuperficiesState>) {
		if (newState.superficies !== undefined) {
			this.superficies.set(newState.superficies);
		}
		if (newState.currentSuperficie !== undefined) {
			this.currentSuperficie.set(newState.currentSuperficie);
		}
		if (newState.error !== undefined) {
			this.error.set(newState.error);
		}
		this.state.update((currentState) => ({ ...currentState, ...newState }));
	}

	public resetState() {
		this.setState(this.initialState);
	}

	public addSuperficie(superficie: Superficie) {
		this.superficies.update((current) => [...current, superficie]);
		this.setState({ superficies: this.superficies() });
	}
	public updateSuperficie(superficie: Superficie) {
		this.superficies.update((current) =>
			current.map((s) => (s.id === superficie.id ? superficie : s))
		);
		this.setState({ superficies: this.superficies() });
	}
}
