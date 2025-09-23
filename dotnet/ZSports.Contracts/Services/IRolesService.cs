namespace ZSports.Contracts.Services;

public interface IRolesService
{
    Task<bool> CreateRoleAsync(string roleName);
    Task<IEnumerable<string>> GetAllRolesAsync();
}
