import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PageHero } from '../../components/page-hero/page-hero';

type SubmitStatus = 'idle' | 'submitting' | 'success';

interface SessionOption {
  id: string;
  title: string;
  day: string;
  time: string;
  status: 'open' | 'full' | 'upcoming';
  statusLabel: string;
}

@Component({
  selector: 'app-registration',
  imports: [PageHero, ReactiveFormsModule],
  templateUrl: './registration.html',
  styleUrl: './registration.css'
})
export class Registration {
  private readonly fb = inject(FormBuilder);

  readonly sessions: SessionOption[] = [
    {
      id: 'research-methods-clinic',
      title: 'Research methods clinic',
      day: 'Day 1',
      time: '11:00',
      status: 'open',
      statusLabel: 'Open'
    },
    {
      id: 'suturing-wound-closure',
      title: 'Suturing & wound closure',
      day: 'Day 1',
      time: '14:00',
      status: 'full',
      statusLabel: 'Full'
    },
    {
      id: 'point-of-care-ultrasound',
      title: 'Point-of-care ultrasound',
      day: 'Day 2',
      time: '14:00',
      status: 'open',
      statusLabel: 'Open'
    },
    {
      id: 'emergency-simulation',
      title: 'Emergency simulation',
      day: 'Day 3',
      time: '09:00',
      status: 'upcoming',
      statusLabel: 'Registration opens soon'
    }
  ];

  readonly form = this.fb.nonNullable.group({
    fullName: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    institution: ['', [Validators.required, Validators.minLength(2)]],
    category: ['student-partner', [Validators.required]],
    sessions: this.fb.nonNullable.group(
      Object.fromEntries(this.sessions.map((s) => [s.id, this.fb.nonNullable.control(false)]))
    )
  });

  readonly status = signal<SubmitStatus>('idle');

  get sessionsGroup(): FormGroup {
    return this.form.get('sessions') as FormGroup;
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.status.set('submitting');

    // TODO: wire to the real backend once there's a public GET /api/events to source session ids from,
    // and a registration endpoint that accepts multiple sessions at once (today's EventRegistrationsController
    // takes one eventId per call).
    setTimeout(() => {
      this.status.set('success');
      this.form.reset({ category: 'student-partner' });
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
