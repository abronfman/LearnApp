using LearnApp.Api.Models;

namespace LearnApp.Api.Services
{
    public class InMemoryContactService : IContactService
    {
        private readonly List<Contact> _contacts =
        [
            new() {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            },
            new() { Id = 2, FirstName = "James", LastName = "Carter", Email = "james.carter@example.com" },
            new() { Id = 3, FirstName = "Aisha", LastName = "Khan",   Email = "aisha.khan@example.com" }
        ];

        public IReadOnlyList<Contact> GetAll(int take) => _contacts.Take(take).ToList();
        public Contact? GetById(int id) => _contacts.FirstOrDefault(c => c.Id == id);

    }
}
