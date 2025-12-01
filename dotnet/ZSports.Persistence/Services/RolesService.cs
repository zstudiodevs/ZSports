using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZSports.Contracts.Services;

namespace ZSports.Persistence.Services;

public class RolesService(RoleManager<IdentityRole<Guid>> roleManager) : IRolesService
{
    public async Task<bool> CreateRoleAsync(string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
            return false;
        var result = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        return result.Succeeded;
    }

    public async Task<IEnumerable<string>> GetAllRolesAsync()
    {
        return await roleManager.Roles.Select(r => r.Name!).ToListAsync();
    }
}
