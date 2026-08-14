import { Component, HostListener, computed, signal } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';

interface Person {
  name: string;
  role: string;
  bio: string;
  initial: string;
}

interface MemberGroup {
  id: string;
  num: string;
  label: string;
  blurb: string;
  people: Person[];
}

@Component({
  selector: 'app-members',
  imports: [PageHero],
  templateUrl: './members.html',
  styleUrl: './members.css'
})
export class Members {
  readonly groups: MemberGroup[] = [
    {
      id: 'cat0',
      num: '01',
      label: 'Student medical team',
      blurb:
        "The students organising this year's congress — programme, logistics, communications and delegate care.",
      people: [
        {
          name: 'Marija Stojanova',
          role: 'Student medical team',
          bio: '4th year medicine — leads delegate communications and the daily on-site schedule.',
          initial: 'M'
        },
        {
          name: 'Petar Angelov',
          role: 'Student medical team',
          bio: '6th year medicine — coordinates volunteers and room logistics across all three days.',
          initial: 'P'
        },
        {
          name: 'Ivana Trpkova',
          role: 'Student medical team',
          bio: '3rd year medicine — manages registration desk and delegate check-in.',
          initial: 'I'
        },
        {
          name: 'Aleksandar Nikolov',
          role: 'Student medical team',
          bio: '5th year medicine — runs the workshop equipment and technical setup.',
          initial: 'A'
        }
      ]
    },
    {
      id: 'cat1',
      num: '02',
      label: 'Professors',
      blurb:
        'Faculty advisors who guide the academic direction of the congress and support the scientific programme.',
      people: [
        {
          name: 'Prof. Biljana Trajkova',
          role: 'Faculty advisor',
          bio: 'Senior lecturer in cardiology and long-standing academic advisor to ICOF.',
          initial: 'B'
        },
        {
          name: 'Prof. Goran Miloševski',
          role: 'Faculty advisor',
          bio: "Dean's office liaison, oversees faculty-level approvals and venue access.",
          initial: 'G'
        },
        {
          name: 'Prof. Ivan Cvetanovski',
          role: 'Research committee',
          bio: 'Chairs the abstract review board for the annual research day.',
          initial: 'I'
        }
      ]
    },
    {
      id: 'cat2',
      num: '03',
      label: 'Scientific team',
      blurb: 'Reviews abstracts, builds the academic programme and briefs speakers ahead of each session.',
      people: [
        {
          name: 'Dr. Elena Georgieva',
          role: 'Cardiology track lead',
          bio: 'Reviews cardiology abstracts and chairs the cardiology session.',
          initial: 'E'
        },
        {
          name: 'Dr. Filip Ristovski',
          role: 'Neurology track lead',
          bio: 'Coordinates the neurology lecture block and speaker briefings.',
          initial: 'F'
        },
        {
          name: 'Dr. Sara Kovačevska',
          role: 'Public health track lead',
          bio: 'Oversees the public health and research day submissions.',
          initial: 'S'
        }
      ]
    },
    {
      id: 'cat3',
      num: '04',
      label: 'Finance & IT',
      blurb:
        'Keeps the congress funded, budgeted and technically running — from sponsorship invoicing to the website itself.',
      people: [
        {
          name: 'Bojan Stefanovski',
          role: 'Finance lead',
          bio: 'Manages the congress budget, invoicing and sponsor payments.',
          initial: 'B'
        },
        {
          name: 'Kristina Naumova',
          role: 'IT & systems',
          bio: 'Runs registration systems, the website and on-site technical support.',
          initial: 'K'
        }
      ]
    },
    {
      id: 'cat4',
      num: '05',
      label: 'Main contributors',
      blurb: 'Long-standing volunteers and former organisers who continue to support ICOF year over year.',
      people: [
        {
          name: 'Ana Petrovska',
          role: 'President',
          bio: "Final-year medical student leading this year's organising committee.",
          initial: 'A'
        },
        {
          name: 'Darko Ilievski',
          role: 'Scientific committee',
          bio: 'Oversees abstract review and the research day programme.',
          initial: 'D'
        },
        {
          name: 'Nina Đorđević',
          role: 'Alumni advisor',
          bio: 'Former president, now advising the current committee.',
          initial: 'N'
        }
      ]
    }
  ];

  readonly activeGroupId = signal(this.groups[0].id);
  readonly activeGroup = computed(
    () => this.groups.find((g) => g.id === this.activeGroupId())!
  );
  readonly selectedPerson = signal<Person | null>(null);

  selectGroup(id: string): void {
    this.activeGroupId.set(id);
  }

  openPerson(person: Person): void {
    this.selectedPerson.set(person);
  }

  closeModal(): void {
    this.selectedPerson.set(null);
  }

  @HostListener('document:keydown.escape')
  onEscape(): void {
    this.closeModal();
  }
}
