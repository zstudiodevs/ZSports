import { CommonModule } from '@angular/common';
import {
	Component,
	effect,
	ElementRef,
	inject,
	ViewChild,
} from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthStore, User, UserRoles } from '../../auth';
import { MatChipsModule } from '@angular/material/chips';
import { ProfileInfoComponent } from './profile-info/profile-info.component';
import { MatTabsModule } from '@angular/material/tabs';
import { OwnerInfoComponent } from './owner-info/owner-info.component';

@Component({
	selector: 'zs-profile',
	templateUrl: './profile.component.html',
	styleUrl: './profile.component.scss',
	standalone: true,
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		MatCardModule,
		MatInputModule,
		MatIconModule,
		MatProgressSpinnerModule,
		MatChipsModule,
		ProfileInfoComponent,
		MatTabsModule,
		OwnerInfoComponent,
	],
})
export class ProfileComponent {
	@ViewChild('loadingRef', { static: true }) loadingRef!: ElementRef;
	private authStore = inject(AuthStore);

	protected user!: User;
	protected username!: string;
	protected isOwner = false;

	constructor() {
		effect(() => {
			if (this.authStore.user$() !== null) this.user = this.authStore.user$()!;

			if (this.authStore.username$() !== null)
				this.username = this.authStore.username$()!;

			this.isOwner =
				this.user?.roles.some((role) => role === UserRoles.Owner) || false;
		});
	}
}
