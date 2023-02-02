import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { ENVIRONMENT } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private _userSubject: BehaviorSubject<User | null>;
  private _user: Observable<User | null>;

  constructor(private router: Router, private http: HttpClient) {
    this._userSubject = new BehaviorSubject(
      JSON.parse(localStorage.getItem('user')!)
    );

    this._user = this._userSubject.asObservable();
  }

  login(username: string, password: string): Observable<User> {
    return this.http
      .post<any>(`${ENVIRONMENT.API_URL}/Security/post/auth/user`, {
        username,
        password,
      })
      .pipe(
        map((user: User) => {
          localStorage.setItem('user', JSON.stringify(user));
          this._userSubject.next(user);

          return user;
        })
      );
  }

  logout(): void {
    localStorage.removeItem('user');
    this._userSubject.next(null);
    this.router.navigate(['/login']);
  }

  public get user(): User | null {
    return this._userSubject.value;
  }
}
