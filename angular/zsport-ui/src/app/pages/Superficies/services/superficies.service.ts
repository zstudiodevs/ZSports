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

	getAll(includeDisabled?: boolean) {
		return this.http.get<Superficie[]>(this.apiUrl, {
			params: { includeDisabled: includeDisabled ?? false },
		});
	}

	getById(id: string) {
		return this.http.get<Superficie>(`${this.apiUrl}/${id}`);
	}

	create(nombre: string) {
		return this.http.post<Superficie>(this.apiUrl, { superficie: nombre });
	}

	update(id: string, nombre: string) {
		return this.http.put<Superficie>(`${this.apiUrl}`, {
			id: id,
			nombre: nombre,
		});
	}

	enable(id: string) {
		return this.http.patch<boolean>(`${this.apiUrl}/enable/${id}`, {});
	}

	disable(id: string) {
		return this.http.patch<boolean>(`${this.apiUrl}/disable/${id}`, {});
	}
}
