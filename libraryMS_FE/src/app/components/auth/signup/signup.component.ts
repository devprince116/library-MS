// src/app/components/signup/signup.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon'; // For eye icon
import { AuthService } from '../../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { LoaderComponent } from '../../loader/loader.component';
import { fadeInOut } from '../../../assets/animations';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule,
    LoaderComponent,
    RouterModule
  ],
  templateUrl: "./signup.component.html",
  styleUrl: "./signup.component.scss",
  animations: [fadeInOut],
})
export class SignupComponent {
  signupForm!: FormGroup;
  loading = false;
  hidePassword = true;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.signupForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['', Validators.required],
    });
  }

  togglePasswordVisibility(event: Event) {
    event.preventDefault();
    this.hidePassword = !this.hidePassword;
  }

  onSubmit() {
    if (this.signupForm.valid) {
      this.loading = true;
      const { name, email, password, role } = this.signupForm.value;
      this.authService.register(name!, email!, password!, role!).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.loading = false;
          console.error('Registration failed:', err);
          alert('Registration failed: ' + (err.error || 'Unknown error'));
        },
      });
    }
  }
}