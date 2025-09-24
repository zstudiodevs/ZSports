import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    public isLoggedIn(): boolean {
        const token = localStorage.getItem('authToken');
        return !!token;
    }
}