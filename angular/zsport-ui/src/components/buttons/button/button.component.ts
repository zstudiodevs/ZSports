import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export type ButtonType = 'simple' | 'raised' | 'stroked' | 'flat' | 'icon';
export type ButtonColor = 'primary' | 'accent' | 'warn' | undefined;
export type ButtonHtmlType = 'button' | 'submit' | 'reset';

export type Button = {
	id: string;
	type: ButtonType;
	color: ButtonColor;
	disabled: boolean;
	htmlType: ButtonHtmlType;
	label: string;
	icon?: string;
};

@Component({
	selector: 'zs-button',
	templateUrl: './button.component.html',
	styleUrl: './button.component.scss',
	imports: [CommonModule, MatButtonModule, MatIconModule],
})
export class ButtonComponent {
	@Input() id: string = '';
	@Input() buttonType: ButtonType = 'simple';
	@Input() color: ButtonColor = undefined;
	@Input() disabled = false;
	@Input() type: ButtonHtmlType = 'button';
	@Input() label = '';
	@Input() icon?: string;
	@Output() onClick = new EventEmitter<void>();
}
