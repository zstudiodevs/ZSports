import {
	Overlay,
	OverlayRef,
	FlexibleConnectedPositionStrategy,
} from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { inject, Injectable, ElementRef } from '@angular/core';
import { LoadingComponent } from '../loading.component';

@Injectable({
	providedIn: 'root',
})
export class LoadingService {
	private overlay = inject(Overlay);
	private overlayRef: OverlayRef | null = null;

	/**
	 * Muestra el overlay de loading sobre el elemento referenciado
	 * @param target ElementRef del elemento a cubrir
	 */
	public show(target: ElementRef) {
		if (this.overlayRef) return;
		const positionStrategy: FlexibleConnectedPositionStrategy = this.overlay
			.position()
			.flexibleConnectedTo(target)
			.withPositions([
				{
					originX: 'start',
					originY: 'top',
					overlayX: 'start',
					overlayY: 'top',
				},
			]);
		this.overlayRef = this.overlay.create({
			hasBackdrop: false,
			positionStrategy,
			width: target.nativeElement.offsetWidth,
			height: target.nativeElement.offsetHeight,
		});
		this.overlayRef.attach(new ComponentPortal(LoadingComponent));
	}

	/**
	 * Oculta el overlay de loading
	 */
	public hide() {
		this.overlayRef?.dispose();
		this.overlayRef = null;
	}
}
