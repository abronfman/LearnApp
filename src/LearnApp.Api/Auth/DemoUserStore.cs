namespace LearnApp.Api.Auth;

public record DemoUser(string Id, string Name, string Email, string Role);

public interface IDemoUserStore
{
    DemoUser? FindByCredentials(string email, string password);
}

// DEMO ONLY: plain-text passwords in code. Real systems store salted password hashes
// (ASP.NET Core Identity does this for you) or hand login to an identity provider.
public class DemoUserStore : IDemoUserStore
{
    private readonly List<(DemoUser User, string Password)> _users =
    [
        (new DemoUser("1", "Ana Admin",   "admin@learnapp.local",  "Admin"),       "Admin123!"),
        (new DemoUser("2", "Carl Case",   "case@learnapp.local",   "CaseManager"), "Case123!"),
        (new DemoUser("3", "Vera Viewer", "viewer@learnapp.local", "Viewer"),      "Viewer123!"),
    ];

    public DemoUser? FindByCredentials(string email, string password) =>
        _users.FirstOrDefault(u =>
            string.Equals(u.User.Email, email, StringComparison.OrdinalIgnoreCase) && u.Password == password).User;
}
