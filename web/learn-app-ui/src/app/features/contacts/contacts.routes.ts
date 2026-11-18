import { Routes } from '@angular/router';

// The contacts feature's own routes. Each page is a separate JavaScript chunk,
// downloaded the first time someone visits it.
export const CONTACT_ROUTES: Routes = [
  {
    path: '',
    title: 'Contacts',
    loadComponent: () => import('./contact-list-page/contact-list-page').then(m => m.ContactListPage),
  },
  {
    path: ':id',
    title: 'Contact details',
    loadComponent: () => import('./contact-detail-page/contact-detail-page').then(m => m.ContactDetailPage),
  },
];
