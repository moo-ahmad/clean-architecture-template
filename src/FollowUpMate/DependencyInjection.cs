using FollowUpMate.Application;
using FollowUpMate.Infrastructure;

namespace FollowUpMate.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddApplicationDI()
                    .AddInfrastructureDI();
            return services;
        }
    }
}
