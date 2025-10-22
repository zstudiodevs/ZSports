import { Component, inject } from '@angular/core';
import { ToolbarComponent } from '../toolbar/toolbar.component';
import { IconButton } from '@app/shared/buttons/models';
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
	secondaryButtons: IconButton[] = [
		{
			id: 'close',
			icon: 'close',
			buttonType: 'icon',
			htmlType: 'button',
			disabled: false,
			action: () => this.closeSidebar(),
		},
	];

	menuItems: { label: string; icon: string; route: string }[] = [
		{ label: 'Inicio', icon: 'home', route: '/' },
		{ label: 'Superficies', icon: 'square_foot', route: '/superficies' },
		{ label: 'Canchas', icon: 'sports_soccer', route: '/canchas' },
	];

	closeSidebar() {
		this.sidebarService.closeSidebar();
	}
}
