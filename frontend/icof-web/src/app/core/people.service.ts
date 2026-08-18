import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from './api-config';

export interface TeamMemberDto {
  id: string;
  slug: string;
  fullName: string;
  roleTitle: string | null;
  institution: string | null;
  shortBio: string | null;
  bio: string | null;
  photoUrl: string | null;
  displayOrder: number;
}

export interface PeopleGroupDto {
  id: string;
  slug: string;
  name: string;
  description: string | null;
  displayOrder: number;
  members: TeamMemberDto[];
}

export type PeopleGroupType = 'MemberGroup' | 'AmbassadorGroup' | 'ContributorGroup';

@Injectable({ providedIn: 'root' })
export class PeopleService {
  private readonly http = inject(HttpClient);

  getGroups(type: PeopleGroupType): Observable<PeopleGroupDto[]> {
    return this.http.get<PeopleGroupDto[]>(`${API_BASE_URL}/api/people-groups`, {
      params: { type }
    });
  }
}
