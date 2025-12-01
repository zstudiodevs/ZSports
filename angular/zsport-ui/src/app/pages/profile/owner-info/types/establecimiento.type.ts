import { User } from '../../../../auth';

export type CrearEstablecimiento = {
	nombre: string;
	descripcion: string;
	telefono: string;
	email: string;
	activo: boolean;
	propietarioId: string;
};

export type Establecimiento = {
	id: string;
	nombre: string;
	descripcion: string;
	telefono: string;
	email: string;
	activo: boolean;
	propietario: User;
};

export type UpdateEstablecimiento = {
	id: string;
	descripcion: string;
	telefono: string;
	email: string;
};

export type EstablecimientoState = {
	currentEstablecimiento: Establecimiento | null;
	error: string | null;
};
