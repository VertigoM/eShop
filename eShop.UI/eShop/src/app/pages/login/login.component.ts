import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private _auth: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    if (this._auth.user) {
      router.navigate(['/home']);
    }
  }

  loginForm!: FormGroup;
  submitted: boolean = false;
  errorMessage: string = '';
  hide: boolean = true;
  loading: boolean = false;

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this._auth
      .login(this.f['email'].value, this.f['password'].value)
      .pipe(first())
      .subscribe({
        next: () => {
          const returnUrl =
            this.route.snapshot.queryParams['returnUrl'] || 'home';
          this.router.navigateByUrl(returnUrl);
        },
        error: (error: any) => {
          if (error instanceof HttpErrorResponse) {
            if (error.status == 401) {
              this.errorMessage = 'Invalid Credentials.';
            } else {
              this.errorMessage = 'Something went wrong.';
            }
          }
        },
        complete: () => {
          this.loading = false;
        },
      });
  }

  get f(): any {
    return this.loginForm.controls;
  }
}
