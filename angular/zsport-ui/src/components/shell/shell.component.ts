import { CommonModule } from '@angular/common';
import { Component, Input, input, OnInit, Output, output } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { BreakpointObserver } from '@angular/cdk/layout';
import {
	Button,
	ButtonComponent,
} from '@components/buttons/button/button.component';

@Component({
	selector: 'zs-shell',
	templateUrl: './shell.component.html',
	styleUrl: './shell.component.scss',
	imports: [CommonModule, MatToolbarModule, MatSidenavModule, ButtonComponent],
})
export class ShellComponent implements OnInit {
	sidenavMode: 'side' | 'over' = 'side';
	isSidenavOpened = true;
	isDarkTheme = true;

	@Input() secondaryButtons: Button[] = [];
	public secondaryButtonClicked = output<string>();

	constructor(private breakpointObserver: BreakpointObserver) {}

	ngOnInit() {
		this.breakpointObserver
			.observe(['(max-width: 600px)'])
			.subscribe((result) => {
				this.sidenavMode = result.matches ? 'over' : 'side';
				this.isSidenavOpened = !result.matches;
			});
	}
}
