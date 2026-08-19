import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

type SubmitStatus = 'idle' | 'submitting' | 'success' | 'error';
import { GoogleMapsModule } from '@angular/google-maps';
@Component({
  selector: 'app-contact',
  imports: [RouterLink, ReactiveFormsModule, GoogleMapsModule],
  standalone: true,
  templateUrl: './contact.html',
  styleUrl: './contact.css',
})
export class Contact {
  private readonly fb = inject(FormBuilder);

  readonly form = this.fb.nonNullable.group({
    fullName: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    location: [''],
    message: ['', [Validators.required, Validators.minLength(10)]],
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
  mapCenter = { lat: 41.9965, lng: 21.4315 };
  mapZoom = 15;

  mapOptions: google.maps.MapOptions = {
    disableDefaultUI: false,
    styles: [
      { elementType: 'geometry', stylers: [{ color: '#0c1a2c' }] },
      { elementType: 'labels.text.stroke', stylers: [{ color: '#0c1a2c' }] },
      { elementType: 'labels.text.fill', stylers: [{ color: '#8b96ab' }] },
      {
        featureType: 'administrative.locality',
        elementType: 'labels.text.fill',
        stylers: [{ color: '#d9c39a' }],
      },
      { featureType: 'poi', elementType: 'labels.text.fill', stylers: [{ color: '#8b96ab' }] },
      { featureType: 'poi.park', elementType: 'geometry', stylers: [{ color: '#132a1e' }] },
      { featureType: 'poi.park', elementType: 'labels.text.fill', stylers: [{ color: '#6b8a76' }] },
      { featureType: 'road', elementType: 'geometry', stylers: [{ color: '#17293f' }] },
      { featureType: 'road', elementType: 'geometry.stroke', stylers: [{ color: '#0c1a2c' }] },
      { featureType: 'road', elementType: 'labels.text.fill', stylers: [{ color: '#8b96ab' }] },
      { featureType: 'road.highway', elementType: 'geometry', stylers: [{ color: '#1e3550' }] },
      {
        featureType: 'road.highway',
        elementType: 'geometry.stroke',
        stylers: [{ color: '#081221' }],
      },
      {
        featureType: 'road.highway',
        elementType: 'labels.text.fill',
        stylers: [{ color: '#d9c39a' }],
      },
      { featureType: 'transit', elementType: 'geometry', stylers: [{ color: '#17293f' }] },
      {
        featureType: 'transit.station',
        elementType: 'labels.text.fill',
        stylers: [{ color: '#8b96ab' }],
      },
      { featureType: 'water', elementType: 'geometry', stylers: [{ color: '#081221' }] },
      { featureType: 'water', elementType: 'labels.text.fill', stylers: [{ color: '#4a5c73' }] },
    ],
  };
}
