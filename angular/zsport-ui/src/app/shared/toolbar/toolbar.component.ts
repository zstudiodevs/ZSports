import { Component, input } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import {
	Button,
	ButtonComponent,
	DropdownButton,
	DropdownButtonComponent,
	IconButton,
} from '@app/shared/buttons';
import { IconButtonComponent } from "../buttons/icon-button/icon-button.component";

@Component({
	selector: 'zs-toolbar',
	templateUrl: './toolbar.component.html',
	styleUrl: './toolbar.component.scss',
	standalone: true,
	imports: [
    MatToolbarModule,
    ButtonComponent,
    DropdownButtonComponent,
    IconButtonComponent
],
})
export class ToolbarComponent {
	title = input<string>();
	mainButton = input<Button>();
	secondaryButtons = input<(Button | IconButton | DropdownButton)[]>();

	protected isDropdownButton(button: any): boolean {
		return (button as DropdownButton).items !== undefined;
	}

	protected asDropdownButton(button: Button | IconButton | DropdownButton): DropdownButton | null {
		return this.isDropdownButton(button) ? (button as DropdownButton) : null;
	}

	protected isButton(button: any): boolean {
		return (button as Button).label !== undefined || (button as Button).icon !== undefined;
	}

	protected asButton(button: Button | IconButton | DropdownButton): Button | null {
		return this.isButton(button) ? (button as Button) : null;
	}

	protected isIconButton(button: any): boolean {
		return (button as IconButton).icon !== undefined && ('label' in button) === false;
	}

	protected asIconButton(button: any): IconButton | null {
		return this.isIconButton(button) ? (button as IconButton) : null;
	}
}
