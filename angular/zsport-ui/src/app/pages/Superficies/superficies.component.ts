import { Component, effect, inject } from '@angular/core';
import { SuperficiesService } from './services/superficies.service';
import { SuperficiesStore } from './store/superficies.store';
import { Superficie } from './types/superficies.types';
import { ToolbarComponent } from '@app/shared/toolbar/toolbar.component';
import { Button } from '@app/shared/buttons';

@Component({
	selector: 'zs-superficies',
	templateUrl: './superficies.component.html',
	styleUrl: './superficies.component.scss',
	standalone: true,
	imports: [ToolbarComponent],
})
export class SuperficiesComponent {
	private readonly superficiesService = inject(SuperficiesService);
	private readonly superficiesStore = inject(SuperficiesStore);

	protected title = 'Superficies';
	protected backButton: Button = {
		id: 'back',
		icon: 'arrow_back',
		buttonType: 'icon',
		htmlType: 'button',
		disabled: false,
		action: () => window.history.back(),
	};
	protected superficies: Superficie[] = [];

	constructor() {
		effect(() => {
			this.superficiesService.getAll().subscribe({
				next: (superficies) => {
					this.superficiesStore.setState({ superficies });
				},
				error: (error) => this.superficiesStore.setState({ error }),
			});
		});

		effect(() => (this.superficies = this.superficiesStore.superficies$()));
	}
}
