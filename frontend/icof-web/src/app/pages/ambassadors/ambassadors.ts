import { Component, HostListener, OnInit, inject, signal } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';
import { PeopleService } from '../../core/people.service';

interface Ambassador {
  name: string;
  country: string;
  bio: string;
  initial: string;
  image?: string;
}

@Component({
  selector: 'app-ambassadors',
  imports: [PageHero],
  templateUrl: './ambassadors.html',
  styleUrl: './ambassadors.css'
})
export class Ambassadors implements OnInit {
  private readonly peopleService = inject(PeopleService);

  readonly ambassadors = signal<Ambassador[]>([]);
  readonly selectedAmbassador = signal<Ambassador | null>(null);

  ngOnInit(): void {
    this.peopleService.getGroups('AmbassadorGroup').subscribe({
      next: (groups) => {
        const people = groups.flatMap((g) => g.members).map((m) => ({
          name: m.fullName,
          // RoleTitle carries the country for ambassadors (e.g. "Serbia").
          country: m.roleTitle ?? '',
          bio: m.bio ?? m.shortBio ?? '',
          initial: m.fullName.charAt(0).toUpperCase(),
          image: m.photoUrl ?? undefined
        }));
        this.ambassadors.set(people);
      },
      error: (err) => console.error('Failed to load ambassadors', err)
    });
  }

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
