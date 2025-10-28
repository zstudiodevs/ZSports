import { CommonModule } from '@angular/common';
import {
	Component,
	computed,
	effect,
	ElementRef,
	inject,
	signal,
	ViewChild,
} from '@angular/core';
import {
	FormControl,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule, MatLabel } from '@angular/material/input';
import { NavigationService } from '@app/services/navigation.service';
import { Button, ButtonComponent } from '@app/shared/buttons';
import { Subject, takeUntil } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoadingService } from '@app/shared/loading/services/loading.service';
import { AuthStore } from '@app/auth/store/auth.store';
import { UpdateUserRequest, User, UserRoles } from '@app/auth/types/auth.type';
import { SnackbarService } from '@app/shared/snackbar/services/snackbar.service';
import { AuthService } from '@app/auth/auth.service';
import { MatChipsModule } from '@angular/material/chips';
import { ProfileInfoComponent } from "./profile-info/profile-info.component";
import { MatTabsModule } from '@angular/material/tabs';

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
		MatTabsModule
	],
})
export class ProfileComponent {
	@ViewChild('loadingRef', { static: true }) loadingRef: ElementRef;
	protected isOwner = true;
}
