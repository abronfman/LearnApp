import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ContactCard } from '../contact-card/contact-card';
import { ContactStore } from '../contact-store';

@Component({
  selector: 'app-contact-list-page',
  imports: [ContactCard],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <h2>Contacts <span class="muted">({{ store.count() }})</span></h2>

    <label for="search">Search</label>
    <input id="search" type="search" placeholder="Name or email"
           [value]="store.search()" (input)="onSearch($event)" />

    <div class="grid">
      @for (c of store.summaries(); track c.id) {
        <app-contact-card [contact]="c" (deleteRequested)="onDelete($event)" />
      } @empty {
        <p>No contacts match "{{ store.search() }}".</p>
      }
    </div>
  `,
  styles: `
    label { display: block; font-weight: 600; margin-bottom: 4px; }
    input { padding: 6px 8px; width: 280px; margin-bottom: 16px; }
    .grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(240px, 1fr)); gap: 12px; }
    .muted { color: #666; font-weight: normal; }
  `,
})
export class ContactListPage {
  // inject() instead of a constructor parameter: same DI, less ceremony.
  protected readonly store = inject(ContactStore);

  protected onSearch(event: Event): void {
    this.store.setSearch((event.target as HTMLInputElement).value);
  }

  protected onDelete(id: number): void {
    this.store.remove(id);
  }
}
