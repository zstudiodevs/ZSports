import { Component, ElementRef, inject, input } from "@angular/core";
import { LoadingService } from "@app/shared/loading/services/loading.service";
import { MatCardModule } from "@angular/material/card";
import { Establecimiento } from "./types/institution.type";

@Component({
    selector: 'zs-owner-info',
    templateUrl: './owner-info.component.html',
    styleUrl: './owner-info.component.scss',
    standalone: true,
    imports: [MatCardModule],
})
export class OwnerInfoComponent {
    public loadingRef = input.required<ElementRef>();
    private readonly loadingService = inject(LoadingService);

    protected establecimiento: Establecimiento;
    
    constructor() {

    }
}