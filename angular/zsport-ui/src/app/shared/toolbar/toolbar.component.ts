import { Component, input } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import {
	Button,
	ButtonComponent,
	DropdownButton,
	DropdownButtonComponent,
	IconButton,
} from '@app/shared/buttons';
import { IconButtonComponent } from '../buttons/icon-button/icon-button.component';

@Component({
	selector: 'zs-toolbar',
	templateUrl: './toolbar.component.html',
	styleUrl: './toolbar.component.scss',
	standalone: true,
	imports: [
		MatToolbarModule,
		ButtonComponent,
		IconButtonComponent,
		DropdownButtonComponent,
	],
})
export class ToolbarComponent {
	title = input<string>();
	mainButton = input<Button>();
	secondaryButtons = input<(Button | IconButton | DropdownButton)[]>();
}
