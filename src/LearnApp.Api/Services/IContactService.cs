using LearnApp.Api.Models;

namespace LearnApp.Api.Services
{
    public interface IContactService
    {
        IReadOnlyList<Contact> GetAll(int take);
        Contact? GetById(int id);
    }
}
