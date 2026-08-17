import { Component, input } from '@angular/core';

export type ItemStatus = 'open' | 'upcoming' | 'closed' | 'full';

export interface ProgrammeItem {
  title: string;
  description: string;
  day: string;
  time: string;
  room: string;
  status?: ItemStatus;
  statusLabel?: string;
}

@Component({
  selector: 'app-item-list',
  imports: [],
  templateUrl: './item-list.html',
  styleUrl: './item-list.css'
})
export class ItemList {
  items = input.required<ProgrammeItem[]>();
}
