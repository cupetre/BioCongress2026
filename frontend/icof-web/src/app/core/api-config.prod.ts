// Swapped in for src/app/core/api-config.ts automatically on production builds via
// angular.json's fileReplacements — so `ng serve` (dev) keeps hitting localhost, while
// `docker compose up --build` (which runs a production build) hits the real deployed API.
// Update this when the real domain/Azure URL replaces the raw Hetzner IP.
export const API_BASE_URL = 'http://78.46.249.178:8080';
