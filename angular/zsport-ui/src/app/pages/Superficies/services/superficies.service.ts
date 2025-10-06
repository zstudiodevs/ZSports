import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { Superficie } from '../types/superficies.types';

@Injectable({
	providedIn: 'root',
})
export class SuperficiesService {
	private apiUrl = environment.apiUrl + '/superficies';
	private readonly http = inject(HttpClient);

	getAll() {
		return this.http.get<Superficie[]>(this.apiUrl);
	}
}
