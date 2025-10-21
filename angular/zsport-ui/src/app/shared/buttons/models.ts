export type ButtonType =
	| 'simple'
	| 'raised'
	| 'stroked'
	| 'flat'
	| 'icon'
	| 'fab'
	| 'mini-fab';
export type ButtonColor =
	| 'primary'
	| 'accent'
	| 'warn'
	| 'success'
	| 'info'
	| 'danger'
	| undefined;
export type ButtonHtmlType = 'button' | 'submit' | 'reset';

export type Button = {
	id: string;
	buttonType: ButtonType;
	disabled: boolean;
	htmlType: ButtonHtmlType;
	label?: string;
	icon?: string;
	color?: ButtonColor;
	action: () => void;
};

export type IconButton = Button & {
	icon: string;
};

export type DropdownButton = {
	id: string;
	label?: string;
	icon?: string;
	buttonType: ButtonType;
	htmlType: ButtonHtmlType;
	disabled: boolean;
	items: Button[];
};
