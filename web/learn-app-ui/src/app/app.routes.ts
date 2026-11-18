import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'contacts' },
  {
    path: 'contacts',
    // The whole feature's route table is loaded lazily too.
    loadChildren: () => import('./features/contacts/contacts.routes').then(m => m.CONTACT_ROUTES),
  },
  { path: '**', redirectTo: 'contacts' },  // anything unknown goes to the list; always last
];
