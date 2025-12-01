import { Injectable, signal } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class SidebarService {
	private sidebarOpened = signal<boolean>(false);
	public isSidebarOpened = this.sidebarOpened.asReadonly();

	private sidebarMode = signal<'side' | 'over'>('over');
	public getSidebarMode = this.sidebarMode.asReadonly();

	toggleSidebar() {
		this.sidebarOpened.set(!this.sidebarOpened());
	}

	closeSidebar() {
		this.sidebarOpened.set(false);
	}

	setSidebarMode(mode: 'side' | 'over') {
		this.sidebarMode.set(mode);
	}
}
