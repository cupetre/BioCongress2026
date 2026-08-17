import { Component } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';
import { ItemList, ProgrammeItem } from '../../components/item-list/item-list';

@Component({
  selector: 'app-social-programs',
  imports: [PageHero, ItemList],
  templateUrl: './social-programs.html',
  styleUrl: './social-programs.css'
})
export class SocialPrograms {
  readonly events: ProgrammeItem[] = [
    {
      title: 'Registration & welcome coffee',
      description: 'Check in, collect your badge and delegate pack before the congress opens.',
      day: 'Day 1',
      time: '09:00',
      room: 'Main lobby'
    },
    {
      title: 'Welcome reception',
      description: 'Informal networking for all delegates, faculty and speakers to kick off the congress.',
      day: 'Day 1',
      time: '18:00',
      room: 'Faculty courtyard'
    },
    {
      title: 'Closing keynote & awards',
      description: 'Congress highlights and the awards ceremony, recognising this year’s top abstracts.',
      day: 'Day 3',
      time: '19:00',
      room: 'Main auditorium'
    },
    {
      title: 'Closing gala',
      description: 'An evening of celebration to close the congress — dinner, music and one last chance to connect.',
      day: 'Day 3',
      time: '20:30',
      room: 'Faculty courtyard'
    }
  ];
}
