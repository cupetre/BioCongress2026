import { Component, OnInit, inject, signal } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';
import { ItemList, ProgrammeItem } from '../../components/item-list/item-list';
import { EventDto, EventsService } from '../../core/events.service';
import { toDayLabel, toTimeLabel } from '../../core/congress-dates';

function toProgrammeItem(e: EventDto): ProgrammeItem {
  return {
    id: e.id,
    title: e.title,
    description: e.summary ?? '',
    day: toDayLabel(e.startsAtUtc),
    time: toTimeLabel(e.startsAtUtc),
    room: e.room ?? ''
  };
}

@Component({
  selector: 'app-social-programs',
  imports: [PageHero, ItemList],
  templateUrl: './social-programs.html',
  styleUrl: './social-programs.css'
})
export class SocialPrograms implements OnInit {
  private readonly eventsService = inject(EventsService);

  readonly events = signal<ProgrammeItem[]>([]);

  ngOnInit(): void {
    this.eventsService.getEvents('Session').subscribe({
      next: (events) => this.events.set(events.map(toProgrammeItem)),
      error: (err) => console.error('Failed to load social programs', err)
    });
  }
}
