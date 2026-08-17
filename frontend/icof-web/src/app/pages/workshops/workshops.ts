import { Component } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';
import { ItemList, ProgrammeItem } from '../../components/item-list/item-list';

@Component({
  selector: 'app-workshops',
  imports: [PageHero, ItemList],
  templateUrl: './workshops.html',
  styleUrl: './workshops.css'
})
export class Workshops {
  readonly workshops: ProgrammeItem[] = [
    {
      title: 'Research methods clinic',
      description: 'Structuring a research abstract from raw data to submission-ready copy. Small group, hands-on.',
      day: 'Day 1',
      time: '11:00',
      room: 'Room C1',
      status: 'open',
      statusLabel: 'Open'
    },
    {
      title: 'Suturing & wound closure',
      description: 'Hands-on surgical skills lab covering basic suturing technique and wound-closure principles.',
      day: 'Day 1',
      time: '14:00',
      room: 'Room B2',
      status: 'full',
      statusLabel: 'Full'
    },
    {
      title: 'Point-of-care ultrasound',
      description: 'An introduction to bedside ultrasound, with supervised scanning practice in small groups.',
      day: 'Day 2',
      time: '14:00',
      room: 'Room B3',
      status: 'open',
      statusLabel: 'Open'
    },
    {
      title: 'Emergency simulation',
      description: 'A simulated trauma scenario in the skills lab, followed by a structured debrief with faculty.',
      day: 'Day 3',
      time: '09:00',
      room: 'Simulation centre',
      status: 'upcoming',
      statusLabel: 'Registration opens soon'
    }
  ];
}
