import { User } from "@app/auth/types/auth.type";

export type Establecimiento = {
    id: string;
    nombre: string;
    descripcion: string;
    telefono: string;
    email: string;
    activo: boolean;
    propietario: User;
}