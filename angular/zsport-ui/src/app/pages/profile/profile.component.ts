import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule, MatLabel } from "@angular/material/input";
import { ButtonComponent } from "@components/buttons/button/button.component";

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
        MatLabel,
        ButtonComponent,
    ]
})
export class ProfileComponent {

}