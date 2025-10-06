export type ButtonType = 'simple' | 'raised' | 'stroked' | 'flat';
export type IconButtonType = 'icon' | 'fab' | 'mini-fab';
export type ButtonColor = 'primary' | 'accent' | 'warn' | undefined;
export type ButtonHtmlType = 'button' | 'submit' | 'reset';

export type Button = {
	id: string;
	buttonType: ButtonType;
	disabled: boolean;
	htmlType: ButtonHtmlType;
	label?: string;
	icon?: string;
	action: () => void;
};

export type IconButton = {
	id: string;
	icon: string;
	buttonType: IconButtonType;
	htmlType: ButtonHtmlType;
	disabled: boolean;
	action: () => void;
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
