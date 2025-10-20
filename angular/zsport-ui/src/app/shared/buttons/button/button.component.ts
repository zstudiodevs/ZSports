import {
	Component,
	EventEmitter,
	input,
	Input,
	output,
	Output,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ButtonColor, ButtonHtmlType, ButtonType } from '../models';

@Component({
	selector: 'zs-button',
	templateUrl: './button.component.html',
	styleUrl: './button.component.scss',
	imports: [MatButtonModule, MatIconModule],
})
export class ButtonComponent {
	id = input.required<string>();
	label = input<string>();
	icon = input<string>();
	buttonType = input<ButtonType>('simple');
	htmlType = input<ButtonHtmlType>('button');
	disabled = input<boolean>(false);
	color = input<ButtonColor>('primary');
	action = input.required<() => void>();
	click = output<void>();

	onClick() {
		if (!this.disabled) {
			this.action();
			this.click.emit();
		}
	}
}
