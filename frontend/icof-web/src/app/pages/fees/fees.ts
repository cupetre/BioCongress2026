import { Component } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';

interface FeeRow {
  num: string;
  category: string;
  includes: string;
  price: string;
  tag?: string;
}

@Component({
  selector: 'app-fees',
  imports: [PageHero],
  templateUrl: './fees.html',
  styleUrl: './fees.css'
})
export class Fees {
  readonly fees: FeeRow[] = [
    {
      num: '01',
      category: 'Student — partner faculty',
      includes:
        'Full congress access — all lectures, workshops you register for, the research day and the social programme.',
      price: '€60',
      tag: 'Most common'
    },
    {
      num: '02',
      category: 'Student — non-partner faculty',
      includes: 'Full congress access, on the same terms as partner-faculty students.',
      price: '€85'
    },
    {
      num: '03',
      category: 'Young professional',
      includes: 'Full congress access, including networking events aimed at early-career clinicians.',
      price: '€110'
    },
    {
      num: '04',
      category: 'Workshop add-on',
      includes:
        'One additional hands-on workshop, booked alongside any registration category. Subject to availability.',
      price: '€15'
    },
    {
      num: '05',
      category: 'Single day pass',
      includes: 'One day of your choice — lectures and open sessions only, workshops not included.',
      price: '€35'
    }
  ];
}
