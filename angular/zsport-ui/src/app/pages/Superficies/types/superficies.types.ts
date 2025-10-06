export interface Superficie {
	id: string;
	nombre: string;
}

export type SuperficiesState = {
	superficies: Superficie[];
	currentSuperficie: Superficie | null;
	error: string | null;
};
