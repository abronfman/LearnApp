import { ChangeDetectionStrategy, Component, computed, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ContactStore } from '../contact-store';

@Component({
  selector: 'app-contact-detail-page',
  imports: [RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <a routerLink="/contacts">&larr; Back to contacts</a>

    @if (contact(); as c) {
      <h2>{{ c.firstName }} {{ c.lastName }}</h2>
      <dl>
        <dt>Email</dt><dd>{{ c.email }}</dd>
        <dt>Phone</dt><dd>{{ c.phone ?? 'None on file' }}</dd>
      </dl>

      <h3>Addresses</h3>
      <ul>
        @for (a of c.addresses; track a.id) {
          <li>{{ a.street }}, {{ a.city }}, {{ a.state }} {{ a.postalCode }}</li>
        } @empty {
          <li>No addresses on file.</li>
        }
      </ul>
    } @else {
      <p>Contact {{ id() }} was not found.</p>
    }
  `,
  styles: `
    dt { font-weight: 600; }
    dd { margin: 0 0 8px; }
  `,
})
export class ContactDetailPage {
  private readonly store = inject(ContactStore);

  // Filled from the :id route parameter, because 5.7 turns on withComponentInputBinding().
  // Route parameters are always strings.
  readonly id = input.required<string>();

  // Re-runs when the id changes OR when the store's contacts change.
  protected readonly contact = computed(() => this.store.byId(Number(this.id())));
}
