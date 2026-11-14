using LearnApp.Api.Data;
using LearnApp.Api.Dtos;
using LearnApp.Api.Exceptions;
using LearnApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.Api.Services;

public class EfContactService(AppDbContext db) : IContactService
{
    public async Task<PagedResult<ContactSummaryDto>> SearchAsync(ContactQuery query, int pageSize, CancellationToken ct = default)
    {
        // Building an IQueryable runs NOTHING yet. Each step adds to the SQL that runs at the end.
        IQueryable<Contact> contacts = db.Contacts;

        // Filter: becomes a parameterized LIKE in SQL.
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();
            contacts = contacts.Where(c =>
                c.FirstName.Contains(term) || c.LastName.Contains(term) || c.Email.Contains(term));
        }

        // Sort: only columns on this allowlist are sortable. Never build ORDER BY from raw user input.
        var desc = string.Equals(query.SortDir, "desc", StringComparison.OrdinalIgnoreCase);
        contacts = query.SortBy?.ToLowerInvariant() switch
        {
            "email" => desc ? contacts.OrderByDescending(c => c.Email) : contacts.OrderBy(c => c.Email),
            "firstname" => desc ? contacts.OrderByDescending(c => c.FirstName) : contacts.OrderBy(c => c.FirstName),
            _ => desc ? contacts.OrderByDescending(c => c.LastName).ThenByDescending(c => c.FirstName)
                                : contacts.OrderBy(c => c.LastName).ThenBy(c => c.FirstName),
        };

        var total = await contacts.CountAsync(ct);          // SQL query 1: how many rows match

        var items = await contacts                           // SQL query 2: just this page
            .Skip((query.Page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ContactSummaryDto(c.Id, c.FirstName + " " + c.LastName, c.Email, c.Addresses.Count))
            .ToListAsync(ct);

        return new PagedResult<ContactSummaryDto>(items, query.Page, pageSize, total);
    }

    public async Task<ContactDetailDto?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Contacts
            .Where(c => c.Id == id)
            .Select(c => new ContactDetailDto(
                c.Id, c.FirstName, c.LastName, c.Email, c.Phone,
                c.Addresses.OrderBy(a => a.Id)
                    .Select(a => new AddressDto(a.Id, a.Street, a.City, a.State, a.PostalCode))
                    .ToList()))
            .FirstOrDefaultAsync(ct);

    public async Task<ContactDetailDto> CreateAsync(ContactRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim();

        // Business rule the attributes can't express. (The unique index is the final safety net.)
        if (await db.Contacts.AnyAsync(c => c.Email == email, ct))
            throw new ConflictException($"A contact with email '{email}' already exists.");

        var contact = new Contact
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            Phone = request.Phone
        };

        db.Contacts.Add(contact);          // tracked as Added
        await db.SaveChangesAsync(ct);     // INSERT; SQL Server generates the Id and EF copies it back

        return (await GetByIdAsync(contact.Id, ct))!;
    }

    public async Task<bool> UpdateAsync(int id, ContactRequest request, CancellationToken ct = default)
    {
        var contact = await db.Contacts.FindAsync([id], ct);   // tracked, so EF detects the changes below
        if (contact is null)
            return false;

        var email = request.Email.Trim();
        if (await db.Contacts.AnyAsync(c => c.Email == email && c.Id != id, ct))
            throw new ConflictException($"A contact with email '{email}' already exists.");

        contact.FirstName = request.FirstName.Trim();
        contact.LastName = request.LastName.Trim();
        contact.Email = email;
        contact.Phone = request.Phone;

        await db.SaveChangesAsync(ct);     // UPDATE only the columns that actually changed
        return true;
    }

    // One DELETE statement, without loading the entity first. The database's cascade rule removes the addresses.
    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
        await db.Contacts.Where(c => c.Id == id).ExecuteDeleteAsync(ct) > 0;
}