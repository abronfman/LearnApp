import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ContactSummary } from '../contact.model';

@Component({
  selector: 'app-contact-card',
  imports: [RouterLink],                            // standalone: list what the template uses
  changeDetection: ChangeDetectionStrategy.OnPush,  // re-check only when an input or signal changes
  template: `
    <article class="card">
      <h3><a [routerLink]="['/contacts', contact().id]">{{ contact().fullName }}</a></h3>
      <p>{{ contact().email }}</p>
      <p class="muted">
        @switch (contact().addressCount) {
          @case (0) { No addresses }
          @case (1) { 1 address }
          @default { {{ contact().addressCount }} addresses }
        }
      </p>
      <button type="button" [attr.aria-label]="'Delete ' + contact().fullName"
              (click)="deleteRequested.emit(contact().id)">
        Delete
      </button>
    </article>
  `,
  styles: `
    .card { border: 1px solid #ccc; border-radius: 8px; padding: 12px 16px; }
    h3 { margin: 0 0 4px; }
    p { margin: 4px 0; }
    .muted { color: #666; font-size: 0.9em; }
  `,
})
export class ContactCard {
  // A signal input: read it by calling it, contact(). required = the parent must bind it.
  readonly contact = input.required<ContactSummary>();

  // An output: the card ANNOUNCES the click. The parent decides what deleting means.
  readonly deleteRequested = output<number>();
}
