import { Component, DestroyRef, OnInit, PLATFORM_ID, computed, inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PeopleService } from '../../core/people.service';

interface TeamCard {
  name: string;
  role: string;
  image?: string;
}

// The 3 people permanently featured in the home page's "Meet the team" rotation —
// currently all 3 live in the Ambassadors group, not the general Members groups.
// Matched by exact full name against whatever comes back from the Ambassadors API.
const FEATURED_NAMES = ['ivana dimitriovska', 'nenad smileski', 'ana kjupeva'];

const ROTATION_INTERVAL_MS = 10000;
const ROTATION_FADE_MS = 380;

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
  private readonly peopleService = inject(PeopleService);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly destroyRef = inject(DestroyRef);

  private readonly teamPool = signal<TeamCard[]>([]);
  private readonly rotation = signal(0);

  // Whole card stack briefly fades out, swaps who's shown, then fades back in —
  // a cheap cross-fade without pulling in Angular's animations module.
  readonly fading = signal(false);

  // Which person sits in the left / main / right slot — shifts by one every tick.
  readonly teamCards = computed(() => {
    const pool = this.teamPool();
    if (pool.length === 0) return [];
    const offset = this.rotation();
    return [0, 1, 2].map((i) => pool[(offset + i) % pool.length]);
  });

  ngOnInit(): void {
    this.peopleService.getGroups('AmbassadorGroup').subscribe({
      next: (groups) => {
        const people = groups.flatMap((g) => g.members);

        const featured = FEATURED_NAMES
          .map((target) => people.find((p) => p.fullName.trim().toLowerCase() === target))
          .filter((p): p is (typeof people)[number] => !!p)
          .map((p) => ({ name: p.fullName, role: p.roleTitle ?? '', image: p.photoUrl ?? undefined }));

        this.teamPool.set(featured);
      },
      error: (err) => console.error('Failed to load team preview', err)
    });

    if (isPlatformBrowser(this.platformId)) {
      let fadeTimeoutId: ReturnType<typeof setTimeout> | undefined;

      const intervalId = setInterval(() => {
        this.fading.set(true);
        fadeTimeoutId = setTimeout(() => {
          this.rotation.update((r) => r + 1);
          this.fading.set(false);
        }, ROTATION_FADE_MS);
      }, ROTATION_INTERVAL_MS);

      this.destroyRef.onDestroy(() => {
        clearInterval(intervalId);
        clearTimeout(fadeTimeoutId);
      });
    }
  }
}
