import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface InfoCard {
  num: string;
  title: string;
  description: string;
  linkLabel: string;
  linkPath: string;
}

@Component({
  selector: 'app-about',
  imports: [RouterLink],
  templateUrl: './about.html',
  styleUrl: './about.css'
})
export class About {
  readonly cards: InfoCard[] = [
    {
      num: '01',
      title: 'Academic programme',
      description:
        'Keynote lectures, research presentations and a dedicated research day, reviewed by our scientific committee.',
      linkLabel: 'See the timetable',
      linkPath: '/timetable'
    },
    {
      num: '02',
      title: 'Hands-on workshops',
      description:
        'Small-group clinical skills sessions run by faculty and visiting specialists, open for registration.',
      linkLabel: 'Browse workshops',
      linkPath: '/workshops'
    },
    {
      num: '03',
      title: 'A growing community',
      description:
        'Student ambassadors, long-term contributors and partner faculties who return edition after edition.',
      linkLabel: 'Meet the team',
      linkPath: '/members'
    }
  ];
}
