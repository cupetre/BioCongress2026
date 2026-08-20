import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';
import { EventDto, EventsService, EventTypeApi } from '../../core/events.service';
import { DayPeriod, toDayHeading, toDayKey, toDayPeriod, toTimeLabel } from '../../core/congress-dates';

type DotType = 'lecture' | 'workshop' | 'social';

interface Entry {
  time: string;
  title: string;
  dot: DotType;
  meta: string;
  period: DayPeriod;
}

interface Day {
  id: string;
  label: string;
  entries: Entry[];
}

const PERIODS: DayPeriod[] = ['Morning', 'Afternoon', 'Evening'];

function toDotType(type: EventTypeApi): DotType {
  switch (type) {
    case 'Lecture':
      return 'lecture';
    case 'Workshop':
      return 'workshop';
    default:
      return 'social'; // Session, Congress
  }
}

@Component({
  selector: 'app-timetable',
  imports: [PageHero],
  templateUrl: './timetable.html',
  styleUrl: './timetable.css',
})
export class Timetable implements OnInit {
  private readonly eventsService = inject(EventsService);

  readonly periods = PERIODS;

  readonly days = signal<Day[]>([]);
  readonly activeDayId = signal<string | null>(null);
  readonly activeDay = computed(
    () => this.days().find((d) => d.id === this.activeDayId()) ?? null
  );

  // 'all' shows every entry for the active day; otherwise filters to one part of the day.
  readonly activePeriod = signal<DayPeriod | 'all'>('all');

  readonly visibleEntries = computed(() => {
    const day = this.activeDay();
    if (!day) return [];
    const period = this.activePeriod();
    return period === 'all' ? day.entries : day.entries.filter((e) => e.period === period);
  });

  // Which periods actually have entries on the active day, so empty modules can be greyed out.
  readonly periodsWithEntries = computed(() => {
    const day = this.activeDay();
    const set = new Set<DayPeriod>();
    if (day) {
      for (const e of day.entries) set.add(e.period);
    }
    return set;
  });

  ngOnInit(): void {
    this.eventsService.getEvents().subscribe({
      next: (events) => this.buildDays(events),
      error: (err) => console.error('Failed to load timetable', err)
    });
  }

  private buildDays(events: EventDto[]): void {
    const sorted = [...events].sort((a, b) => a.startsAtUtc.localeCompare(b.startsAtUtc));
    const dayMap = new Map<string, Day>();

    for (const e of sorted) {
      const key = toDayKey(e.startsAtUtc);
      let day = dayMap.get(key);
      if (!day) {
        day = { id: key, label: toDayHeading(e.startsAtUtc), entries: [] };
        dayMap.set(key, day);
      }
      day.entries.push({
        time: toTimeLabel(e.startsAtUtc),
        title: e.title,
        dot: toDotType(e.type),
        meta: [e.summary, e.room].filter((v) => !!v && v.trim().length > 0).join(' · '),
        period: toDayPeriod(e.startsAtUtc)
      });
    }

    const days = Array.from(dayMap.values());
    this.days.set(days);
    if (days.length > 0) {
      this.activeDayId.set(days[0].id);
    }
  }

  selectDay(id: string): void {
    this.activeDayId.set(id);
    this.activePeriod.set('all');
  }

  selectPeriod(period: DayPeriod | 'all'): void {
    this.activePeriod.set(period);
  }
}
