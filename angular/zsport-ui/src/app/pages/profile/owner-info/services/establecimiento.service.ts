import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '@env/environment';
import {
	CrearEstablecimiento,
	Establecimiento,
} from '../types/establecimiento.type';

@Injectable({
	providedIn: 'root',
})
export class EstablecimientoService {
	private apiUrl = environment.apiUrl + '/establecimientos';
	private http = inject(HttpClient);

	public create(data: CrearEstablecimiento) {
		return this.http.post<Establecimiento>(this.apiUrl, data);
	}

	public getByPropietario(propietarioId: string) {
		var params = new HttpParams().set('propietarioId', propietarioId);
		return this.http.get<Establecimiento[]>(this.apiUrl, { params });
	}
}
