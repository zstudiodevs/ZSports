import { inject, Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
} from '@angular/common/http';
import { firstValueFrom, Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { selectAuthToken } from '../../state/auth/auth.selectors';

export function authTokenInterceptorFn(req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> {
    const store = inject(Store);
    return new Observable<HttpEvent<any>>(observer => {
        firstValueFrom(store.select(selectAuthToken)).then(token => {
            let authHeader = token;
            if (!authHeader) {
                authHeader = localStorage.getItem('authToken') || null;
            }
            let cloned = req;
            if (authHeader) {
                cloned = req.clone({
                    setHeaders: {
                        Authorization: `Bearer ${authHeader}`,
                    },
                });
            } 
            next(cloned).subscribe({
            next: (event) => observer.next(event),
            error: (err) => observer.error(err),
            complete: () => observer.complete()
            });
        });
    });
}
