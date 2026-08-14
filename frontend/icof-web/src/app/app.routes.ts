import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { History } from './pages/history/history';
import { Members } from './pages/members/members';
import { Fees } from './pages/fees/fees';
import { Timetable } from './pages/timetable/timetable';
import { Contact } from './pages/contact/contact';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'history', component: History },
  { path: 'members', component: Members },
  { path: 'fees', component: Fees },
  { path: 'timetable', component: Timetable },
  { path: 'contact', component: Contact }
  // Future pages: about, ambassadors, partners, agenda, workshops, lectures, social-programs,
  // registration, sponsors, awards — added as their own routes here.
];
