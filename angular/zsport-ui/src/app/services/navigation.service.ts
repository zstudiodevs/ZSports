import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
	providedIn: 'root',
})
export class NavigationService {
	constructor(private router: Router) {}

	navigateTo(path: string | any[], params?: { [key: string]: any }) {
		this.router.navigate(Array.isArray(path) ? path : [path], {
			queryParams: params,
		});
	}

	navigateByUrl(url: string) {
		this.router.navigateByUrl(url);
	}

	goBack() {
		window.history.back();
	}

	reload() {
		this.router.navigate([this.router.url]);
	}
}
