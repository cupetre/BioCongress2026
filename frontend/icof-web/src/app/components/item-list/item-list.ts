import { Component, input, output } from '@angular/core';

export type ItemStatus = 'open' | 'upcoming' | 'closed' | 'full';

export interface ProgrammeItem {
  id: string;
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
  isAdmin = input<boolean>(false);

  edit = output<ProgrammeItem>();
  archive = output<ProgrammeItem>();
}
