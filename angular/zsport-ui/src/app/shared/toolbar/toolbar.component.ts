import { Component, input } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import {
	Button,
	ButtonComponent,
	DropdownButton,
	DropdownButtonComponent,
} from '@components/buttons';

@Component({
	selector: 'zs-toolbar',
	templateUrl: './toolbar.component.html',
	styleUrl: './toolbar.component.scss',
	standalone: true,
	imports: [MatToolbarModule, ButtonComponent, DropdownButtonComponent],
})
export class ToolbarComponent {
	title = input<string>();
	mainButton = input<Button>();
	secondaryButtons = input<(Button | DropdownButton)[]>();
}
