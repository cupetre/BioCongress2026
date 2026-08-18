// Single source of truth for "which congress day is this" — matches the "Oct 14-16, 2027"
// dates shown on the Timetable page and seeded on the backend (Data/DataSeeder.cs).
const CONGRESS_START_DATE = '2027-10-14';

function utcMidnight(date: Date): number {
  return Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate());
}

/** "Day 1" / "Day 2" / "Day 3", relative to the fixed congress start date. */
export function toDayLabel(isoDateTime: string): string {
  const start = utcMidnight(new Date(CONGRESS_START_DATE));
  const event = utcMidnight(new Date(isoDateTime));
  const dayNumber = Math.round((event - start) / 86_400_000) + 1;
  return `Day ${dayNumber}`;
}

/** "09:00" — hours/minutes taken as UTC, matching how times were seeded. */
export function toTimeLabel(isoDateTime: string): string {
  const d = new Date(isoDateTime);
  const hh = String(d.getUTCHours()).padStart(2, '0');
  const mm = String(d.getUTCMinutes()).padStart(2, '0');
  return `${hh}:${mm}`;
}

/** "Day 1 · Oct 14" — used for the Timetable page's day tabs. */
export function toDayHeading(isoDateTime: string): string {
  const d = new Date(isoDateTime);
  const formatted = new Intl.DateTimeFormat('en-US', {
    month: 'short',
    day: 'numeric',
    timeZone: 'UTC'
  }).format(d);
  return `${toDayLabel(isoDateTime)} · ${formatted}`;
}

/** Stable per-day grouping key (date-only, UTC), independent of display formatting. */
export function toDayKey(isoDateTime: string): string {
  return isoDateTime.slice(0, 10);
}
