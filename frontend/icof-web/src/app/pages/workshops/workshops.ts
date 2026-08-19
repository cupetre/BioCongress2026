import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';
import { ItemList, ItemStatus, ProgrammeItem } from '../../components/item-list/item-list';
import { AdminEventForm } from '../../components/admin-event-form/admin-event-form';
import { EventDto, EventsService, EventStatusApi } from '../../core/events.service';
import { toDayLabel, toTimeLabel } from '../../core/congress-dates';
import { AuthService } from '../../core/auth.service';

const STATUS_MAP: Partial<Record<EventStatusApi, ItemStatus>> = {
  Open: 'open',
  Full: 'full',
  Upcoming: 'upcoming',
  Closed: 'closed'
};

const STATUS_LABELS: Record<ItemStatus, string> = {
  open: 'Open',
  full: 'Full',
  upcoming: 'Registration opens soon',
  closed: 'Closed'
};

function toProgrammeItem(e: EventDto): ProgrammeItem {
  const status = STATUS_MAP[e.status] ?? 'closed';
  return {
    id: e.id,
    title: e.title,
    description: e.summary ?? '',
    day: toDayLabel(e.startsAtUtc),
    time: toTimeLabel(e.startsAtUtc),
    room: e.room ?? '',
    status,
    statusLabel: STATUS_LABELS[status]
  };
}

@Component({
  selector: 'app-workshops',
  imports: [PageHero, ItemList, AdminEventForm],
  templateUrl: './workshops.html',
  styleUrl: './workshops.css'
})
export class Workshops implements OnInit {
  private readonly eventsService = inject(EventsService);
  private readonly auth = inject(AuthService);

  readonly isAdmin = this.auth.isAdmin;

  private readonly rawEvents = signal<EventDto[]>([]);
  readonly workshops = computed(() => this.rawEvents().map(toProgrammeItem));

  /** null = closed, undefined = create mode, an EventDto = edit mode. */
  readonly formState = signal<'closed' | 'create' | EventDto>('closed');

  ngOnInit(): void {
    this.refresh();
  }

  private refresh(): void {
    this.eventsService.getEvents('Workshop').subscribe({
      next: (events) => this.rawEvents.set(events),
      error: (err) => console.error('Failed to load workshops', err)
    });
  }

  openCreate(): void {
    this.formState.set('create');
  }

  openEdit(item: ProgrammeItem): void {
    const event = this.rawEvents().find((e) => e.id === item.id);
    if (event) {
      this.formState.set(event);
    }
  }

  closeForm(): void {
    this.formState.set('closed');
  }

  onSaved(): void {
    this.formState.set('closed');
    this.refresh();
  }

  archive(item: ProgrammeItem): void {
    if (!confirm(`Archive "${item.title}"? It will disappear from the public site.`)) {
      return;
    }

    this.eventsService.updateEvent(item.id, { isPublished: false }).subscribe({
      next: () => this.refresh(),
      error: (err) => console.error('Failed to archive event', err)
    });
  }

  editingEvent(): EventDto | null {
    const state = this.formState();
    return state === 'closed' || state === 'create' ? null : state;
  }
}
