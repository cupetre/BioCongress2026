import { HttpClient } from '@angular/common/http';
import { Injectable, PLATFORM_ID, computed, inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Observable, tap } from 'rxjs';
import { API_BASE_URL } from './api-config';

interface LoginResponse {
  tokenType: string;
  accessToken: string;
  expiresIn: number;
  refreshToken: string;
}

interface CurrentUser {
  email: string;
  roles: string[];
}

const TOKEN_STORAGE_KEY = 'icof_access_token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly platformId = inject(PLATFORM_ID);

  private readonly accessToken = signal<string | null>(this.readStoredToken());
  private readonly currentUserSignal = signal<CurrentUser | null>(null);

  readonly isLoggedIn = computed(() => this.accessToken() !== null);
  readonly isAdmin = computed(() => this.currentUserSignal()?.roles.includes('Admin') ?? false);
  readonly currentUser = this.currentUserSignal.asReadonly();

  constructor() {
    // On a full page reload there's a stored token but no user info yet — fetch it once.
    if (this.accessToken()) {
      this.loadCurrentUser();
    }
  }

  getAccessToken(): string | null {
    return this.accessToken();
  }

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${API_BASE_URL}/login?useCookies=false`, { email, password })
      .pipe(
        tap((response) => {
          this.accessToken.set(response.accessToken);
          if (isPlatformBrowser(this.platformId)) {
            localStorage.setItem(TOKEN_STORAGE_KEY, response.accessToken);
          }
          this.loadCurrentUser();
        })
      );
  }

  logout(): void {
    this.accessToken.set(null);
    this.currentUserSignal.set(null);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(TOKEN_STORAGE_KEY);
    }
  }

  private loadCurrentUser(): void {
    this.http.get<CurrentUser>(`${API_BASE_URL}/api/auth/me`).subscribe({
      next: (user) => this.currentUserSignal.set(user),
      // Token expired/invalid — drop it rather than pretend we're still logged in.
      error: () => this.logout()
    });
  }

  private readStoredToken(): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    return localStorage.getItem(TOKEN_STORAGE_KEY);
  }
}
