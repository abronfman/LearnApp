using LearnApp.Api.Dtos;
using LearnApp.Api.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnApp.Api.Services;

public interface IContactService
{
    Task<PagedResult<ContactSummaryDto>> SearchAsync(ContactQuery query, int pageSize, CancellationToken ct = default);
    Task<ContactDetailDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ContactDetailDto> CreateAsync(ContactRequest request, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, ContactRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
