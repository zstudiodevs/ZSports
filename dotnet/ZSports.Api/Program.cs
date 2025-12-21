using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ZSports.Contracts.Repositories;
using ZSports.Contracts.Services;
using ZSports.Domain.Entities;
using ZSports.Persistence;
using ZSports.Persistence.Repositories;
using ZSports.Persistence.Services;

namespace ZSports.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddUserSecrets<Program>();

        // Add services to the container.
        builder.Services.AddDbContext<ZSportsDbContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            assembly => assembly.MigrationsAssembly(typeof(ZSportsDbContext).Assembly.FullName))
                .UseLazyLoadingProxies());

        builder.Services.AddIdentity<Usuario, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ZSportsDbContext>()
            .AddDefaultTokenProviders();

        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        builder.Services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        builder.Services.AddScoped<ISuperficieRepository, SuperficieRepository>();
        builder.Services.AddScoped<ICanchasRepository, CanchasRepository>();
        builder.Services.AddScoped<ITurnosRepository, TurnosRepository>();
        builder.Services.AddScoped<ISuperficieService, SuperficieService>();
        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddScoped<IRolesService, RolesService>();
        builder.Services.AddScoped<IDeportesService, DeportesService>();
        builder.Services.AddScoped<IEstablecimientosService, EstablecimientosService>();
        builder.Services.AddScoped<ICanchasService, CanchasService>();
        builder.Services.AddScoped<ITurnosService, TurnosService>();

        // Health Checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ZSportsDbContext>(
                name: "database",
                tags: ["db", "sql", "sqlserver"]);
        builder.Services.AddControllers();

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // OpenAPI/Swagger - habilitado en todos los ambientes usando el nuevo sistema de .NET 10
        builder.Services.AddOpenApi();

        var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
        builder.Services.AddCors(opts =>
        {
            opts.AddPolicy("CorsPolicy", policy =>
            {
                policy.WithOrigins(origins)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        var app = builder.Build();

        // Aplicar migraciones automáticamente al inicio
        using (var scope = app.Services.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                var db = scope.ServiceProvider.GetRequiredService<ZSportsDbContext>();
                var pending = db.Database.GetPendingMigrations();
                if (pending.Any())
                {
                    logger.LogInformation("Aplicando {Count} migraciones pendientes...", pending.Count());
                    db.Database.Migrate();
                    logger.LogInformation("Migraciones aplicadas correctamente.");
                }
                else
                {
                    logger.LogInformation("No hay migraciones pendientes.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error aplicando migraciones al iniciar la aplicación.");
                throw;
            }
        }

        // Configure the HTTP request pipeline.
        // OpenAPI habilitado en todos los ambientes
        app.UseSwagger();
        app.UseSwaggerUI(opts =>
        {
            opts.SwaggerEndpoint("/swagger/v1/swagger.json", "ZSports API V1");
        });

        // Health Check endpoints
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var result = System.Text.Json.JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description,
                        duration = e.Value.Duration.ToString(),
                        exception = e.Value.Exception?.Message,
                        data = e.Value.Data
                    }),
                    totalDuration = report.TotalDuration.ToString()
                });
                await context.Response.WriteAsync(result);
            }
        });

        // Health check simplificado (solo status)
        app.MapHealthChecks("/health/ready");

        app.UseCors("CorsPolicy");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}