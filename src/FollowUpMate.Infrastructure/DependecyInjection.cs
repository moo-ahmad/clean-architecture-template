using FollowUpMate.Application.Interfaces;
using FollowUpMate.Application.Interfaces.Auth;
using FollowUpMate.Application.Interfaces.Dapper;
using FollowUpMate.Application.Interfaces.Repository;
using FollowUpMate.Application.Logging;
using FollowUpMate.Infrastructure.Dapper;
using FollowUpMate.Infrastructure.Data;
using FollowUpMate.Infrastructure.Identity;
using FollowUpMate.Infrastructure.Logging;
using FollowUpMate.Infrastructure.Repositories;
using FollowUpMate.Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FollowUpMate.Infrastructure
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
