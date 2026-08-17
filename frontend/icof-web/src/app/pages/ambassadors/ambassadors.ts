import { Component, HostListener, signal } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';

interface Ambassador {
  name: string;
  country: string;
  bio: string;
  initial: string;
}

@Component({
  selector: 'app-ambassadors',
  imports: [PageHero],
  templateUrl: './ambassadors.html',
  styleUrl: './ambassadors.css'
})
export class Ambassadors {
  readonly ambassadors: Ambassador[] = [
    {
      name: 'Elena Petkovska',
      country: 'North Macedonia',
      bio: 'Coordinates the host-faculty delegation and helps first-time delegates find their way around the venue.',
      initial: 'E'
    },
    {
      name: 'Marko Jovanović',
      country: 'Serbia',
      bio: 'Promotes ICOF at the Belgrade Faculty of Medicine and organises the travelling delegate group.',
      initial: 'M'
    },
    {
      name: 'Yana Dimitrova',
      country: 'Bulgaria',
      bio: 'Runs delegate recruitment in Sofia and Plovdiv, and liaises with the scientific committee on abstracts.',
      initial: 'Y'
    },
    {
      name: 'Dimitris Papadopoulos',
      country: 'Greece',
      bio: 'Builds partnerships with Greek medical faculties and coordinates joint research submissions.',
      initial: 'D'
    },
    {
      name: 'Erisa Hoxha',
      country: 'Albania',
      bio: 'Leads outreach in Tirana and supports Albanian delegates with travel and accommodation questions.',
      initial: 'E'
    },
    {
      name: 'Blerta Krasniqi',
      country: 'Kosovo',
      bio: 'Coordinates the Pristina delegate group and represents ICOF at regional student conferences.',
      initial: 'B'
    },
    {
      name: 'Andrei Popescu',
      country: 'Romania',
      bio: 'Organises the Bucharest and Cluj delegations and helps first-year students prepare their first abstract.',
      initial: 'A'
    },
    {
      name: 'Ivana Kovač',
      country: 'Croatia',
      bio: 'Runs the Zagreb ambassador network and coordinates joint workshops with visiting faculty.',
      initial: 'I'
    }
  ];

  readonly selectedAmbassador = signal<Ambassador | null>(null);

  open(ambassador: Ambassador): void {
    this.selectedAmbassador.set(ambassador);
  }

  close(): void {
    this.selectedAmbassador.set(null);
  }

  @HostListener('document:keydown.escape')
  onEscape(): void {
    this.close();
  }
}
