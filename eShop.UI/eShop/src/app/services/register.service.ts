import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ENVIRONMENT } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  constructor(private http: HttpClient) {}

  register(
    email: string,
    password: string,
    confirmPassword: string
  ): Observable<any> {
    return this.http
      .post<any>(`${ENVIRONMENT.API_URL}/Security/post/register/user`, {
        email,
        password,
        confirmPassword,
      })
      .pipe(
        map((response) => {
          console.log(response);
        })
      );
  }
}
