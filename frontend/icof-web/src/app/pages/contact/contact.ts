import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import {GoogleMapsModule} from '@angular/google-maps';

type SubmitStatus = 'idle' | 'submitting' | 'success' | 'error';



@Component({
  selector: 'app-contact',
  imports: [RouterLink, ReactiveFormsModule ,GoogleMapsModule ],

  templateUrl: './contact.html',
  styleUrl: './contact.css'
})
export class Contact {
  private readonly fb = inject(FormBuilder);

  readonly form = this.fb.nonNullable.group({
    fullName: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    location: [''],
    message: ['', [Validators.required, Validators.minLength(10)]]
  });

  readonly status = signal<SubmitStatus>('idle');

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.status.set('submitting');

    // TODO: swap for a real POST once the backend exposes a contact endpoint (e.g. POST /api/contact).
    setTimeout(() => {
      this.status.set('success');
      this.form.reset();
    }, 900);
  }

  dismissSuccess(): void {
    this.status.set('idle');
  }

  isInvalid(controlName: string): boolean {
    const control = this.form.get(controlName);
    return !!control && control.invalid && (control.dirty || control.touched);
  }
}
