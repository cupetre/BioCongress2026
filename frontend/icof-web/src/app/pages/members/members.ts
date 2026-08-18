import { Component, HostListener, OnInit, computed, inject, signal } from '@angular/core';
import { PageHero } from '../../components/page-hero/page-hero';
import { PeopleService } from '../../core/people.service';

interface Person {
  name: string;
  role: string;
  bio: string;
  initial: string;
  image?: string;
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
export class Members implements OnInit {
  private readonly peopleService = inject(PeopleService);

  readonly groups = signal<MemberGroup[]>([]);
  readonly activeGroupId = signal<string | null>(null);
  readonly activeGroup = computed(
    () => this.groups().find((g) => g.id === this.activeGroupId()) ?? null
  );
  readonly selectedPerson = signal<Person | null>(null);

  ngOnInit(): void {
    this.peopleService.getGroups('MemberGroup').subscribe({
      next: (dtoGroups) => {
        const mapped: MemberGroup[] = dtoGroups.map((g, index) => ({
          id: g.id,
          num: String(index + 1).padStart(2, '0'),
          label: g.name,
          blurb: g.description ?? '',
          people: g.members.map((m) => ({
            name: m.fullName,
            role: m.roleTitle ?? '',
            bio: m.bio ?? m.shortBio ?? '',
            initial: m.fullName.charAt(0).toUpperCase(),
            image: m.photoUrl ?? undefined
          }))
        }));

        this.groups.set(mapped);
        if (mapped.length > 0) {
          this.activeGroupId.set(mapped[0].id);
        }
      },
      error: (err) => console.error('Failed to load members', err)
    });
  }

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
