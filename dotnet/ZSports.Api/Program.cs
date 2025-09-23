
using Microsoft.EntityFrameworkCore;
using ZSports.Api.Services;
using ZSports.Contracts.Repositories;
using ZSports.Contracts.Services;
using ZSports.Persistence;
using ZSports.Persistence.Repositories;

namespace ZSports.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddUserSecrets<Program>();

            // Add services to the container.
            builder.Services.AddDbContext<ZSportsDbContext>(opts =>
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), assembly => assembly.MigrationsAssembly(typeof(ZSportsDbContext).Assembly.FullName)));

            builder.Services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddTransient<ISuperficieRepository, SuperficieRepository>();
            builder.Services.AddTransient<ISuperficieService, SuperficieService>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
