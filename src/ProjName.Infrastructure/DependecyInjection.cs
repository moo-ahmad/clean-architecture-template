using ProjName.Application.Interfaces;
using ProjName.Application.Interfaces.Auth;
using ProjName.Application.Interfaces.Dapper;
using ProjName.Application.Interfaces.Repository;
using ProjName.Application.Logging;
using ProjName.Infrastructure.Dapper;
using ProjName.Infrastructure.Data;
using ProjName.Infrastructure.Identity;
using ProjName.Infrastructure.Logging;
using ProjName.Infrastructure.Repositories;
using ProjName.Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjName.Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IDapperRepository>(
                new DapperRepository(configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));

            services.AddAuthentication();

            return services;
        }
    }
}
