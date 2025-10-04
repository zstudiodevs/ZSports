import { Injectable, signal } from '@angular/core';
import { AuthState, User } from '../types/auth.type';

@Injectable({ providedIn: 'root' })
export class AuthStore {
	private initialState: AuthState = {
		token: null,
		refreshToken: null,
		username: null,
		user: null,
		isAuthenticated: false,
	};
	private state = signal<AuthState>(this.initialState);
	public readonly state$ = this.state.asReadonly();

	private token = signal<string | null>(this.initialState.token);
	public readonly token$ = this.token.asReadonly();

	private refreshToken = signal<string | null>(this.initialState.refreshToken);
	public readonly refreshToken$ = this.refreshToken.asReadonly();

	private username = signal<string | null>(this.initialState.username);
	public readonly username$ = this.username.asReadonly();

	private isAuthenticated = signal<boolean>(this.initialState.isAuthenticated);
	public readonly isAuthenticated$ = this.isAuthenticated.asReadonly();

	private user = signal<User | null>(this.initialState.user);
	public readonly user$ = this.user.asReadonly();

	constructor() {
		const localState = localStorage.getItem('authState');
		if (localState) {
			this.setState(JSON.parse(localState));
		}
	}

	public setState(newState: Partial<AuthState>) {
		this.state.update((currentState) => ({ ...currentState, ...newState }));
		if (newState.token !== undefined) {
			this.token.set(newState.token);
		}
		if (newState.refreshToken !== undefined) {
			this.refreshToken.set(newState.refreshToken);
		}
		if (newState.username !== undefined) {
			this.username.set(newState.username);
		}
		if (newState.user !== undefined) {
			this.user.set(newState.user);
		}
		if (newState.isAuthenticated !== undefined) {
			this.isAuthenticated.set(newState.isAuthenticated);
		}
		localStorage.setItem('authState', JSON.stringify(this.state()));
	}

	public resetState() {
		this.setState(this.initialState);
		localStorage.removeItem('authState');
	}
}
