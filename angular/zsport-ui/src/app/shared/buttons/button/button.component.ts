import { Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ButtonColor, ButtonHtmlType, ButtonType } from '../models';
import { NgClass } from '@angular/common';

@Component({
	selector: 'zs-button',
	templateUrl: './button.component.html',
	styleUrl: './button.component.scss',
	imports: [MatButtonModule, MatIconModule, NgClass],
})
export class ButtonComponent {
	id = input.required<string>();
	label = input<string>();
	icon = input<string>();
	buttonType = input<ButtonType>('simple');
	htmlType = input<ButtonHtmlType>('button');
	disabled = input<boolean>(false);
	color = input<ButtonColor>('primary');
	onClick = output<void>();
}
