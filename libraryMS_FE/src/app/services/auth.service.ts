import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../environments/environment';
import { User } from '../models/user.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl;
  private tokenSubject = new BehaviorSubject<string | null>(localStorage.getItem('token'));
  private userSubject = new BehaviorSubject<User | null>(null);

  constructor(private http: HttpClient, private router: Router) { }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Auth/login`, { email, password }).pipe(
      tap(response => {
        localStorage.setItem('token', response.response.token);
        this.tokenSubject.next(response.response.token);
        this.userSubject.next({ id: response.response.userId, role: response.response.role } as User);
      })
    );
  }


  register(name: string, email: string, password: string, role: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Auth/register`, { name, email, password, role }).pipe(
      tap(response => {
        localStorage.setItem('token', response.response.token);
        this.tokenSubject.next(response.response.token);
        this.userSubject.next({ id: response.response.userId, role: response.response.role, name, email } as User);
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.tokenSubject.next(null);
    this.userSubject.next(null);
    this.router.navigate(['/login']);

  }

  getToken(): string | null {
    return this.tokenSubject.value;
  }

  getUser(): User | null {
    return this.userSubject.value;
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  isAdmin(): boolean {
    return this.getUser()?.role === 'Admin';
  }

  isUser(): boolean {
    return this.getUser()?.role === 'User';
  }
}