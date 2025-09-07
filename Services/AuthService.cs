using Foodshare.Models;

namespace Foodshare.Services;

public class AuthService
{
    readonly DbService _db;
    public static User? CurrentUser { get; private set; }

    public AuthService(DbService db) { _db = db; }

    public async Task<User?> Login(string email, string password)
    {
        var h = DbService.Hash(password);
        var u = await _db.Conn.Table<User>().Where(x => x.Email == email && x.PasswordHash == h).FirstOrDefaultAsync();
        CurrentUser = u;
        return u;
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _db.Conn.Table<User>().Where(x => x.Email == email).FirstOrDefaultAsync() != null;
    }

    public async Task<User> Register(User u, string password)
    {
        u.PasswordHash = DbService.Hash(password);
        await _db.Conn.InsertAsync(u);
        CurrentUser = u;
        return u;
    }

    public void Logout() => CurrentUser = null;
}
