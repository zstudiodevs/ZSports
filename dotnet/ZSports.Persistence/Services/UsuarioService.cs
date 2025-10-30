using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZSports.Contracts.Repositories;
using ZSports.Contracts.Services;
using ZSports.Contracts.Usuarios;
using ZSports.Domain.Constants;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Services;

public class UsuarioService(
    UserManager<Usuario> userManager,
    IRepository<RefreshToken, Guid> repository,
    IConfiguration configuration) : IUsuarioService
{
    public async Task<(bool Succeeded, IEnumerable<string> Errors)> RegistrarUsuarioAsync(RegisterUsuarioDto dto)
    {
        var existingUser = await userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return (false, new[] { "El email ya está registrado." });

        var user = new Usuario();
        user.UserName = dto.Username;
        user.Email = dto.Email;

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description));

        var roleResult = await userManager.AddToRoleAsync(user, Roles.Player);
        if (!roleResult.Succeeded)
            return (false, roleResult.Errors.Select(e => e.Description));

        return (true, Enumerable.Empty<string>());
    }
    public async Task<(LoginUsuarioResponseDto? Response, string Error)> LoginAsync(LoginUsuarioDto dto)
    {
        string error = string.Empty;
        try
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            bool userExists = user != null;
            bool passwordCorrect = false;
            if (userExists)
            {
                passwordCorrect = await userManager.CheckPasswordAsync(user, dto.Password);
            }

            if (!userExists)
            {
                error = "Usuario incorrecto";
            }
            if (userExists && !passwordCorrect)
            {
                error = string.IsNullOrEmpty(error) ? "Contraseña incorrecta" : error + ".Contraseña incorrecta";
            }
            if (!userExists || !passwordCorrect)
            {
                return (null, error);
            }

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

            var response = new LoginUsuarioResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.Token,
                Username = user.UserName,
                Usuario = new UsuarioDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Activo = user.Activo,
                    Roles = roles
                }
            };
            return (response, string.Empty);
        }
        catch (Exception ex)
        {
            return (null, "Error crítico: " + ex.Message);
        }
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
            Username = user.UserName,
            Usuario = new UsuarioDto
            {
                Id = user.Id,
                Email = user.Email,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Activo = user.Activo,
                Roles = roles
            }
        };
    }
    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var token = await repository.GetQueryable()
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.Revoked);

        if (token == null)
            return false;

        token.Revoked = true;
        await repository.SaveChangesAsync();
        return true;
    }
    public async Task<UsuarioDto> GetUserByUsername(string username, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user == null)
            throw new KeyNotFoundException("Usuario no encontrado.");
        var roles = await userManager.GetRolesAsync(user);

        return new UsuarioDto
        {
            Id = user.Id,
            Email = user.Email,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Activo = user.Activo,
            Roles = roles
        };
    }
    public async Task<UsuarioDto> UpdateUsuarioAsync(UpdateUsuarioDto request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new KeyNotFoundException("Usuario no encontrado.");

        user.SetNombre(request.Nombre);
        user.SetApellido(request.Apellido);

        if (!string.IsNullOrWhiteSpace(request.Email) && user.Email != request.Email)
            user.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.Nombre) && user.UserName != request.Nombre)
            user.UserName = request.Nombre;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        return new UsuarioDto
        {
            Id = user.Id,
            Email = user.Email,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Activo = user.Activo,
            Roles = await userManager.GetRolesAsync(user)
        };
    }
    public async Task<(bool Succeeded, IEnumerable<string> Errors)> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return (false, new[] { "Usuario no encontrado." });

        var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description));

        return (true, Enumerable.Empty<string>());
    }

    public async Task<(bool Succeeded, IEnumerable<string> Errors)> AddRoleToUser(string email, string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return (false, new[] { "Usuario no encontrado." });
        var result = await userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description));
        return (true, Enumerable.Empty<string>());
    }
}
