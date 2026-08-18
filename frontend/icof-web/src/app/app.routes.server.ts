import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  // These pull live data from the API, so they're rendered per-request rather than
  // prerendered at build time (prerendering would bake in whatever the API returned
  // during `ng build`, which goes stale the moment content changes in the admin panel).
  {
    path: 'members',
    renderMode: RenderMode.Server
  },
  {
    path: 'ambassadors',
    renderMode: RenderMode.Server
  },
  {
    path: 'workshops',
    renderMode: RenderMode.Server
  },
  {
    path: 'lectures',
    renderMode: RenderMode.Server
  },
  {
    path: 'social-programs',
    renderMode: RenderMode.Server
  },
  {
    path: 'timetable',
    renderMode: RenderMode.Server
  },
  {
    path: '**',
    renderMode: RenderMode.Prerender
  }
];
