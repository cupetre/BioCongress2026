import { Component, computed, signal } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';

type DotType = 'lecture' | 'workshop' | 'social';

interface Entry {
  time: string;
  title: string;
  dot: DotType;
  meta: string;
}

interface Day {
  id: string;
  label: string;
  entries: Entry[];
}

@Component({
  selector: 'app-timetable',
  imports: [PageHero],
  templateUrl: './timetable.html',
  styleUrl: './timetable.css'
})
export class Timetable {
  readonly days: Day[] = [
    {
      id: 'day1',
      label: 'Day 1 · Oct 14',
      entries: [
        {
          time: '09:00',
          title: 'Registration & welcome coffee',
          dot: 'social',
          meta: 'Check in and collect your badge. · Main lobby'
        },
        {
          time: '09:30',
          title: 'Opening keynote',
          dot: 'lecture',
          meta: 'The future of clinical research. · Main auditorium'
        },
        {
          time: '11:00',
          title: 'Research methods clinic',
          dot: 'workshop',
          meta: 'Workshop — structuring an abstract. · Room C1'
        },
        {
          time: '14:00',
          title: 'Suturing & wound closure',
          dot: 'workshop',
          meta: 'Hands-on workshop, surgical skills lab. · Room B2'
        },
        {
          time: '18:00',
          title: 'Welcome reception',
          dot: 'social',
          meta: 'Informal networking for all delegates. · Faculty courtyard'
        }
      ]
    },
    {
      id: 'day2',
      label: 'Day 2 · Oct 15',
      entries: [
        {
          time: '09:00',
          title: 'Research day — abstract presentations',
          dot: 'lecture',
          meta: 'Student research across all tracks. · Rooms A1, A2, A3'
        },
        {
          time: '11:00',
          title: 'Cardiology grand round',
          dot: 'lecture',
          meta: 'Case-based lecture and discussion. · Main auditorium'
        },
        {
          time: '14:00',
          title: 'Point-of-care ultrasound',
          dot: 'workshop',
          meta: 'Hands-on workshop, skills lab. · Room B3'
        },
        {
          time: '17:00',
          title: 'Patient lecture',
          dot: 'lecture',
          meta: 'Living with chronic illness. · Room A1'
        }
      ]
    },
    {
      id: 'day3',
      label: 'Day 3 · Oct 16',
      entries: [
        {
          time: '09:00',
          title: 'Emergency simulation',
          dot: 'workshop',
          meta: 'Simulated trauma scenario & debrief. · Simulation centre'
        },
        {
          time: '12:00',
          title: 'Meet the expert sessions',
          dot: 'lecture',
          meta: 'Small-group conversations with faculty. · Rooms A1–A3'
        },
        {
          time: '19:00',
          title: 'Closing keynote & awards',
          dot: 'social',
          meta: 'Congress highlights and awards ceremony. · Main auditorium'
        },
        {
          time: '20:30',
          title: 'Closing gala',
          dot: 'social',
          meta: 'An evening of celebration to close the congress. · Faculty courtyard'
        }
      ]
    }
  ];

  readonly activeDayId = signal(this.days[0].id);
  readonly activeDay = computed(() => this.days.find((d) => d.id === this.activeDayId())!);

  selectDay(id: string): void {
    this.activeDayId.set(id);
  }
}
