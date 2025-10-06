import { Component, input } from '@angular/core';
import { ButtonHtmlType, IconButtonType } from '../models';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
	selector: 'zs-icon-button',
	templateUrl: './icon-button.component.html',
	styleUrl: './icon-button.component.scss',
	standalone: true,
	imports: [MatButtonModule, MatIconModule],
})
export class IconButtonComponent {
	id = input.required<string>();
	icon = input.required<string>();
	type = input.required<ButtonHtmlType>();
	iconType = input.required<IconButtonType>();
	disabled = input<boolean>(false);
	action = input.required<() => void>();
}
