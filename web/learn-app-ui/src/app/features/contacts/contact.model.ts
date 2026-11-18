// These mirror the API's DTOs in src/LearnApp.Api/Dtos, field for field.
// ASP.NET Core serializes C# PascalCase properties as camelCase JSON, so FullName arrives as fullName.

export interface ContactSummary {
  id: number;
  fullName: string;
  email: string;
  addressCount: number;
}

export interface Address {
  id: number;
  street: string;
  city: string;
  state: string;
  postalCode: string;
}

export interface ContactDetail {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string | null;
  addresses: Address[];
}
