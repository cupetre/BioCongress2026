import { Component } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';

interface LectureEntry {
  title: string;
  description: string;
  day: string;
  time: string;
  room: string;
}

@Component({
  selector: 'app-lectures',
  imports: [PageHero],
  templateUrl: './lectures.html',
  styleUrl: './lectures.css'
})
export class Lectures {
  readonly lectures: LectureEntry[] = [
    {
      title: 'Opening keynote',
      description: 'The future of clinical research — an opening address setting the tone for the congress.',
      day: 'Day 1',
      time: '09:30',
      room: 'Main auditorium'
    },
    {
      title: 'Research day — abstract presentations',
      description: 'Student research presented across all tracks, reviewed live by the scientific committee.',
      day: 'Day 2',
      time: '09:00',
      room: 'Rooms A1, A2, A3'
    },
    {
      title: 'Cardiology grand round',
      description: 'A case-based lecture and open discussion led by the cardiology track faculty.',
      day: 'Day 2',
      time: '11:00',
      room: 'Main auditorium'
    },
    {
      title: 'Patient lecture',
      description: 'Living with chronic illness — a patient perspective session on long-term care.',
      day: 'Day 2',
      time: '17:00',
      room: 'Room A1'
    },
    {
      title: 'Meet the expert sessions',
      description: 'Small-group conversations with faculty across specialties — no registration required.',
      day: 'Day 3',
      time: '12:00',
      room: 'Rooms A1–A3'
    }
  ];
}
