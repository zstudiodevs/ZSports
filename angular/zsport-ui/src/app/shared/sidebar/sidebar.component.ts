import { Component, inject } from '@angular/core';
import { ToolbarComponent } from '../toolbar/toolbar.component';
import { Button } from '@components/buttons/models';
import { SidebarService } from './services/sidebar.service';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
	selector: 'zs-sidebar',
	templateUrl: './sidebar.component.html',
	styleUrls: ['./sidebar.component.scss'],
	standalone: true,
	imports: [
		ToolbarComponent,
		MatListModule,
		MatDividerModule,
		RouterModule,
		MatIconModule,
	],
})
export class SidebarComponent {
	private sidebarService = inject(SidebarService);

	title = 'Menu';
	secondaryButtons: Button[] = [
		{
			id: 'close',
			icon: 'close',
			type: 'icon',
			htmlType: 'button',
			disabled: false,
			action: () => this.closeSidebar(),
		},
	];

	menuItems: { label: string; icon: string; route: string }[] = [
		{ label: 'Inicio', icon: 'home', route: '/' },
	];

	closeSidebar() {
		this.sidebarService.closeSidebar();
	}
}
