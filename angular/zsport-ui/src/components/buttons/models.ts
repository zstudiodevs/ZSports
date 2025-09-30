export type ButtonType = 'simple' | 'raised' | 'stroked' | 'flat' | 'icon';
export type ButtonColor = 'primary' | 'accent' | 'warn' | undefined;
export type ButtonHtmlType = 'button' | 'submit' | 'reset';

export type Button = {
	id: string;
	type: ButtonType;
	disabled: boolean;
	htmlType: ButtonHtmlType;
	label: string;
	icon?: string;
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
