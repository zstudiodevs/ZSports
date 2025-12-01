export interface Superficie {
	id: string;
	nombre: string;
	activo: boolean;
}

export type SuperficiesState = {
	superficies: Superficie[];
	currentSuperficie: Superficie | null;
	error: string | null;
};

export const superficiesColumns: (keyof Superficie)[] = ['nombre'];
