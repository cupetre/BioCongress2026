import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';

export type EventTypeApi = 'Congress' | 'Workshop' | 'Lecture' | 'Session';
export type EventStatusApi = 'Draft' | 'Upcoming' | 'Open' | 'Closed' | 'Full' | 'Completed' | 'Cancelled';

export interface EventDto {
  id: string;
  slug: string;
  title: string;
  summary: string | null;
  room: string | null;
  location: string | null;
  type: EventTypeApi;
  status: EventStatusApi;
  startsAtUtc: string;
  endsAtUtc: string | null;
  isRegistrationEnabled: boolean;
  capacity: number;
  registeredCount: number;
  displayOrder: number;
}

@Injectable({ providedIn: 'root' })
export class EventsService {
  private readonly http = inject(HttpClient);

  getEvents(type?: EventTypeApi): Observable<EventDto[]> {
    let params = new HttpParams();
    if (type) {
      params = params.set('type', type);
    }
    return this.http.get<EventDto[]>(`${API_BASE_URL}/api/events`, { params });
  }
}
