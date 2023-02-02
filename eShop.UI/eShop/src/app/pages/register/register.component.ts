import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { RegisterService } from 'src/app/services/register.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  registrationForm!: FormGroup;
  loading: boolean = false;
  submitted: boolean = false;
  errorMessage?: string;
  hide: boolean = true;

  constructor(
    private formBuilder: FormBuilder,
    private register: RegisterService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.registrationForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    });
  }

  get f(): any {
    return this.registrationForm.controls;
  }

  onSubmit(): void {
    if (this.registrationForm.invalid) {
      return;
    }

    this.loading = true;
    this.register
      .register(
        this.f['email'].value,
        this.f['password'].value,
        this.f['confirmPassword'].value
      )
      .pipe(first())
      .subscribe({
        next: () => {
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
          this.router.navigateByUrl(returnUrl);
        },
        error: (error: any) => {
          if (error instanceof HttpErrorResponse) {
            if (error.status == 400) {
              this.errorMessage = 'Username already in use.';
            } else {
              this.errorMessage = 'Something went wrong.';
            }
          }
        },
        complete: () => {
          this.loading = false;
          this.router.navigate(['/login']);
        },
      });
  }
}
