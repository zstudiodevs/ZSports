export type SnackbarType = 'success' | 'danger' | 'info' | 'warning';
export interface SnackbarConfig {
	message: string;
	duration?: number;
	type?: SnackbarType;
}
