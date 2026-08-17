import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-page-hero',
  imports: [RouterLink],
  templateUrl: './page-hero.html',
  styleUrl: './page-hero.css'
})
export class PageHero {
  section = input.required<string>();
  title = input.required<string>();
  kicker = input.required<string>();
  description = input.required<string>();
  variant = input<'navy' | 'light'>('navy');
}
