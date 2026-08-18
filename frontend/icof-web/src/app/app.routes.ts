import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { About } from './pages/about/about';
import { History } from './pages/history/history';
import { Ambassadors } from './pages/ambassadors/ambassadors';
import { Members } from './pages/members/members';
import { Workshops } from './pages/workshops/workshops';
import { Lectures } from './pages/lectures/lectures';
import { SocialPrograms } from './pages/social-programs/social-programs';
import { Timetable } from './pages/timetable/timetable';
import { Registration } from './pages/registration/registration';
import { Fees } from './pages/fees/fees';
import { Contact } from './pages/contact/contact';
import { Login } from './pages/login/login';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: Login },
  { path: 'about', component: About },
  { path: 'history', component: History },
  { path: 'ambassadors', component: Ambassadors },
  { path: 'members', component: Members },
  { path: 'workshops', component: Workshops },
  { path: 'lectures', component: Lectures },
  { path: 'social-programs', component: SocialPrograms },
  { path: 'timetable', component: Timetable },
  { path: 'registration', component: Registration },
  { path: 'fees', component: Fees },
  { path: 'contact', component: Contact }
  // Later: partners, agenda, sponsors, awards — added as their own routes here.
];
