import { Component, input } from '@angular/core';
import { ButtonHtmlType, ButtonType } from '../models';
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
	htmlType = input.required<ButtonHtmlType>();
	buttonType = input.required<ButtonType>();
	disabled = input<boolean>(false);
	action = input.required<() => void>();
}
