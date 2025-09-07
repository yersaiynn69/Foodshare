using Foodshare.Models;
using SQLite;

namespace Foodshare.Services
{
    public sealed class AuthService
    {
        private static readonly Lazy<AuthService> _lazy = new(() => new AuthService());
        public static AuthService I => _lazy.Value;

        private const string KEY_USER_ID = "auth_user_id";

        public async Task<bool> LoginAsync(string email, string password)
        {
            var u = await DbService.I.GetUserByEmailAsync(email);
            if (u == null) return false;
            if (u.PasswordHash != password) return false;
            Preferences.Set(KEY_USER_ID, u.Id);
            await (Application.Current.MainPage as AppShell)!.BuildTabsForAsync(u.Role);
            return true;
        }

        public Task LogoutAsync()
        {
            Preferences.Remove(KEY_USER_ID);
            return Task.CompletedTask;
        }

        public async Task<User?> GetCurrentAsync()
        {
            var id = Preferences.Get(KEY_USER_ID, string.Empty);
            if (string.IsNullOrEmpty(id)) return null;
            return await DbService.I.GetUserByIdAsync(id);
        }

        // Унифицированная регистрация
        private async Task<bool> RegisterAsync(User u, string password)
        {
            if (string.IsNullOrWhiteSpace(u.Email) || string.IsNullOrWhiteSpace(password)) return false;
            var exists = await DbService.I.GetUserByEmailAsync(u.Email);
            if (exists != null) return false;
            u.PasswordHash = password;
            await DbService.I.UpsertUserAsync(u);
            Preferences.Set(KEY_USER_ID, u.Id);
            await (Application.Current.MainPage as AppShell)!.BuildTabsForAsync(u.Role);
            return true;
        }

        public Task<bool> RegisterRestaurantAsync(string org, string bin, string phone, string email, string city, string addr, string pass) =>
            RegisterAsync(new User
            {
                OrgName = org, BinIin = bin, Phone = phone, Email = email,
                City = city, Address = addr, Role = UserRole.Restaurant
            }, pass);

        public Task<bool> RegisterNgoAsync(string org, string bin, string phone, string email, string city, string addr, string pass) =>
            RegisterAsync(new User
            {
                OrgName = org, BinIin = bin, Phone = phone, Email = email,
                City = city, Address = addr, Role = UserRole.Ngo
            }, pass);

        public Task<bool> RegisterNeedyAsync(string fullName, string phone, string email, string city, string addr, string pass) =>
            RegisterAsync(new User
            {
                FullName = fullName, Phone = phone, Email = email,
                City = city, Address = addr, Role = UserRole.Needy
            }, pass);

        public Task<bool> RegisterVolunteerAsync(string fullName, string phone, string email, string city, string transport, string pass) =>
            RegisterAsync(new User
            {
                FullName = fullName, Phone = phone, Email = email,
                City = city, Address = transport, Role = UserRole.Volunteer
            }, pass);

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var u = await DbService.I.GetUserByEmailAsync(email);
            if (u == null) return false;
            u.PasswordHash = newPassword;
            await DbService.I.UpsertUserAsync(u);
            return true;
        }
    }
}
