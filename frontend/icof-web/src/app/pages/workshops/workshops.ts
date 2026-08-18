import { Component, OnInit, inject, signal } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';
import { ItemList, ItemStatus, ProgrammeItem } from '../../components/item-list/item-list';
import { EventDto, EventsService, EventStatusApi } from '../../core/events.service';
import { toDayLabel, toTimeLabel } from '../../core/congress-dates';

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
  imports: [PageHero, ItemList],
  templateUrl: './workshops.html',
  styleUrl: './workshops.css'
})
export class Workshops implements OnInit {
  private readonly eventsService = inject(EventsService);

  readonly workshops = signal<ProgrammeItem[]>([]);

  ngOnInit(): void {
    this.eventsService.getEvents('Workshop').subscribe({
      next: (events) => this.workshops.set(events.map(toProgrammeItem)),
      error: (err) => console.error('Failed to load workshops', err)
    });
  }
}
