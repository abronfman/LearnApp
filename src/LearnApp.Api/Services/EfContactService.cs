using LearnApp.Api.Data;
using LearnApp.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.Api.Services
{
    public class EfContactService(AppDbContext db) : IContactService
    {
        public async Task<IReadOnlyList<ContactSummaryDto>> GetAllAsync(int take, CancellationToken ct = default) => 
            await db.Contacts.OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                .Take(take)
                .Select(c => new ContactSummaryDto(c.Id, c.FirstName + " " + c.LastName, c.Email, c.Addresses.Count))
                .ToListAsync(ct);

        public async Task<ContactDetailDto?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await db.Contacts.Where(c => c.Id == id)
                .Select(c => new ContactDetailDto(
                    c.Id,
                    c.FirstName,
                    c.LastName,
                    c.Email,
                    c.Addresses.OrderBy(a => a.Id).Select(a => new AddressDto(a.Id, a.Street, a.City, a.State, a.PostalCode)).ToList()
                ))
                .FirstOrDefaultAsync(ct);
    }
}
