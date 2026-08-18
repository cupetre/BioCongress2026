import { Component, OnInit, inject, signal } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';
import { EventDto, EventsService } from '../../core/events.service';
import { toDayLabel, toTimeLabel } from '../../core/congress-dates';

interface LectureEntry {
  title: string;
  description: string;
  day: string;
  time: string;
  room: string;
}

function toLectureEntry(e: EventDto): LectureEntry {
  return {
    title: e.title,
    description: e.summary ?? '',
    day: toDayLabel(e.startsAtUtc),
    time: toTimeLabel(e.startsAtUtc),
    room: e.room ?? ''
  };
}

@Component({
  selector: 'app-lectures',
  imports: [PageHero],
  templateUrl: './lectures.html',
  styleUrl: './lectures.css'
})
export class Lectures implements OnInit {
  private readonly eventsService = inject(EventsService);

  readonly lectures = signal<LectureEntry[]>([]);

  ngOnInit(): void {
    this.eventsService.getEvents('Lecture').subscribe({
      next: (events) => this.lectures.set(events.map(toLectureEntry)),
      error: (err) => console.error('Failed to load lectures', err)
    });
  }
}
