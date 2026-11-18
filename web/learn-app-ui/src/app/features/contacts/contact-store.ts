import { computed, Injectable, signal } from "@angular/core";
import { ContactDetail, ContactSummary } from "./contact.model";

// Sample data, so this step needs no API. Step 6 replaces it with HTTP calls.
const SEED: ContactDetail[] = [
  { id: 1, firstName: 'Ana', lastName: 'Lopez', email: 'ana.lopez@example.com', phone: '555-0101',
    addresses: [{ id: 1, street: '12 Oak St', city: 'Richmond', state: 'VA', postalCode: '23220' }] },
  { id: 2, firstName: 'Ben', lastName: 'Carter', email: 'ben.carter@example.com', phone: null,
    addresses: [] },
  { id: 3, firstName: 'Dana', lastName: 'Kim', email: 'dana.kim@example.com', phone: '555-0103',
    addresses: [
      { id: 2, street: '400 Main St', city: 'Arlington', state: 'VA', postalCode: '22201' },
      { id: 3, street: '9 Lake Rd', city: 'Reston', state: 'VA', postalCode: '20190' },
    ] },
  { id: 4, firstName: 'Eli', lastName: 'Novak', email: 'eli.novak@example.com', phone: '555-0104',
    addresses: [{ id: 4, street: '77 Pine Ave', city: 'Bethesda', state: 'MD', postalCode: '20814' }] },
];

@Injectable({
  providedIn: 'root'
})
export class ContactStore {

  private readonly contacts = signal<ContactDetail[]>(SEED);
  private readonly searchTerm = signal('');

  readonly search = this.searchTerm.asReadonly();

  readonly summaries = computed<ContactSummary[]>(() => {
    const term = this.searchTerm().trim().toLowerCase();

    return this.contacts()
      .filter(c => !term ||  `${c.firstName} ${c.lastName} ${c.email}`.toLowerCase().includes(term))
      .map(c => ({
        id: c.id,
        fullName: `${c.firstName} ${c.lastName}`,
        email: c.email,
        addressCount: c.addresses.length
      }));
  });

  readonly count = computed(() => this.summaries().length);

  setSearch(term: string): void {
    this.searchTerm.set(term);
  }

  remove(id: number): void {
    // A NEW array, never an in-place change. Break 3 shows why.
    this.contacts.update(list => list.filter(c => c.id !== id));
  }

  // Called inside a computed() in the detail page, so it re-runs when contacts change.
  byId(id: number): ContactDetail | undefined {
    return this.contacts().find(c => c.id === id);
  }
}
