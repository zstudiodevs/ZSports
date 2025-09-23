using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZSports.Contracts.Repositories;
using ZSports.Contracts.Services;
using ZSports.Contracts.Usuarios;
using ZSports.Domain.Entities;

namespace ZSports.Api.Services;

public class UsuarioService(
    UserManager<Usuario> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IRepository<RefreshToken, Guid> repository,
    IConfiguration configuration) : IUsuarioService
{
    public async Task<(bool Succeeded, IEnumerable<string> Errors)> RegistrarUsuarioAsync(RegisterUsuarioDto dto)
    {
        if (!await roleManager.RoleExistsAsync(dto.Rol))
            return (false, new[] { $"El rol '{dto.Rol}' no existe." });

        var existingUser = await userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return (false, new[] { "El email ya está registrado." });

        var user = new Usuario();
        user.UserName = dto.Username;
        user.Email = dto.Email;
        user.SetNombre(dto.Nombre);
        user.SetApellido(dto.Apellido);

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description));

        var roleResult = await userManager.AddToRoleAsync(user, dto.Rol);
        if (!roleResult.Succeeded)
            return (false, roleResult.Errors.Select(e => e.Description));

        return (true, Enumerable.Empty<string>());
    }
    public async Task<LoginUsuarioResponseDto?> LoginAsync(LoginUsuarioDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
            return null;

        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var jwtSettings = configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
            Expiration = DateTime.UtcNow.AddDays(7),
            UsuarioId = user.Id
        };

        await repository.AddAsync(refreshToken, default);
        await repository.SaveChangesAsync();

        return new LoginUsuarioResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken.Token,
            Email = user.Email,
            Username = user.UserName,
            Roles = roles
        };
    }
    public async Task<LoginUsuarioResponseDto?> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var storedToken = await repository.GetQueryable()
            .Include(rt => rt.Usuario)
            .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken && !rt.Revoked);

        if (storedToken == null || storedToken.Expiration < DateTime.UtcNow)
            return null;

        // Opcional: revoca el token anterior
        storedToken.Revoked = true;
        await repository.SaveChangesAsync();

        // Genera nuevo access token y refresh token
        var user = storedToken.Usuario;
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(ClaimTypes.Name, user.UserName)
    };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var jwtSettings = configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        // Crea nuevo refresh token
        var newRefreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
            Expiration = DateTime.UtcNow.AddDays(7),
            UsuarioId = user.Id
        };
        await repository.AddAsync(newRefreshToken, default);
        await repository.SaveChangesAsync();

        return new LoginUsuarioResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = newRefreshToken.Token,
            Email = user.Email,
            Username = user.UserName,
            Roles = roles
        };
    }
}
