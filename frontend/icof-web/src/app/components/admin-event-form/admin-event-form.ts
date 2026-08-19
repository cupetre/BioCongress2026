import { Component, effect, inject, input, output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { EventDto, EventsService, EventStatusApi, EventTypeApi } from '../../core/events.service';
import { fromDatetimeLocalValue, toDatetimeLocalValue } from '../../core/congress-dates';

@Component({
  selector: 'app-admin-event-form',
  imports: [ReactiveFormsModule],
  templateUrl: './admin-event-form.html',
  styleUrl: './admin-event-form.css'
})
export class AdminEventForm {
  private readonly fb = inject(FormBuilder);
  private readonly eventsService = inject(EventsService);

  /** Fixed per page — Workshops always creates Workshop-type events, etc. */
  eventType = input.required<EventTypeApi>();

  /** Non-null = editing this event; null = creating a new one. */
  editing = input<EventDto | null>(null);

  saved = output<void>();
  cancelled = output<void>();

  readonly statuses: EventStatusApi[] = ['Draft', 'Upcoming', 'Open', 'Closed', 'Full', 'Completed', 'Cancelled'];
  readonly status = signal<'idle' | 'submitting' | 'error'>('idle');

  readonly form = this.fb.nonNullable.group({
    title: ['', Validators.required],
    summary: [''],
    room: [''],
    location: ['Faculty of Medicine, Skopje'],
    startsAt: ['', Validators.required],
    capacity: [0, [Validators.required, Validators.min(0)]],
    registeredCount: [0, [Validators.required, Validators.min(0)]],
    isRegistrationEnabled: [false],
    eventStatus: ['Open' as EventStatusApi, Validators.required]
  });

  constructor() {
    effect(() => {
      const item = this.editing();
      if (item) {
        this.form.patchValue({
          title: item.title,
          summary: item.summary ?? '',
          room: item.room ?? '',
          location: item.location ?? '',
          startsAt: toDatetimeLocalValue(item.startsAtUtc),
          capacity: item.capacity,
          registeredCount: item.registeredCount,
          isRegistrationEnabled: item.isRegistrationEnabled,
          eventStatus: item.status
        });
      }
    });
  }

  isInvalid(controlName: keyof typeof this.form.controls): boolean {
    const control = this.form.controls[controlName];
    return control.invalid && control.touched;
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.status.set('submitting');
    const v = this.form.getRawValue();

    const payload = {
      title: v.title.trim(),
      summary: v.summary || null,
      room: v.room || null,
      location: v.location || null,
      type: this.eventType(),
      status: v.eventStatus,
      startsAtUtc: fromDatetimeLocalValue(v.startsAt),
      capacity: v.capacity,
      registeredCount: v.registeredCount,
      isRegistrationEnabled: v.isRegistrationEnabled
    };

    const editing = this.editing();
    const request$ = editing
      ? this.eventsService.updateEvent(editing.id, payload)
      : this.eventsService.createEvent(payload);

    request$.subscribe({
      next: () => {
        this.status.set('idle');
        this.saved.emit();
      },
      error: () => this.status.set('error')
    });
  }

  cancel(): void {
    this.cancelled.emit();
  }
}
