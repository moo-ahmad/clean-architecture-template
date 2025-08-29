using FollowUpMate.Application;
using FollowUpMate.Application.Features.Auth.Commands.Login;
using FollowUpMate.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace FollowUpMate.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
            services.AddAuthorization();
            services.AddMediatR(typeof(LoginCommandHandler).Assembly);
            services.AddApplicationDI()
                    .AddInfrastructureDI(configuration);
            return services;
        }
    }
}
