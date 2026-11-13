using LearnApp.Api.Dtos;
using LearnApp.Api.Models;

namespace LearnApp.Api.Services
{
    public interface IContactService
    {
        Task<IReadOnlyList<ContactSummaryDto>> GetAllAsync(int take, CancellationToken ct = default);
        Task<ContactDetailDto?> GetByIdAsync(int id, CancellationToken ct = default);
    }
}
