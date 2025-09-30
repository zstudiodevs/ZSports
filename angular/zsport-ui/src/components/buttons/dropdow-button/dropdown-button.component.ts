import {
	ChangeDetectionStrategy,
	Component,
	Input,
	input,
	OnChanges,
	output,
	SimpleChanges,
} from '@angular/core';
import { Button, ButtonHtmlType, ButtonType } from '../models';
import { CommonModule } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
	selector: 'zs-dropdown-button',
	templateUrl: './dropdown-button.component.html',
	styleUrl: './dropdown-button.component.scss',
	standalone: true,
	imports: [CommonModule, MatMenuModule, MatButtonModule, MatIconModule],
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DropdownButtonComponent implements OnChanges {
	id = input.required<string>();
	label = input<string>();
	icon = input<string>();
	buttonType = input.required<ButtonType>();
	htmlType = input.required<ButtonHtmlType>();
	disabled = input.required<boolean>();
	@Input()
	items: Button[] = [];

	optionClicked = output<string>();

	ngOnChanges(changes: SimpleChanges): void {
		if (changes['items']) {
			this.items = changes['items'].currentValue || [];
		}
	}
}
