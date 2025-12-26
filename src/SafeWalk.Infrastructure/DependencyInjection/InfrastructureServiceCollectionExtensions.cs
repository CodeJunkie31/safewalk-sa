using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafeWalk.Application.Interfaces.Infrastructure;
using SafeWalk.Application.Interfaces.Persistence;
using SafeWalk.Infrastructure.Persistence;
using SafeWalk.Infrastructure.Services;

namespace SafeWalk.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Connection factory
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJourneyRepository, JourneyRepository>();
            services.AddScoped<ITrustedContactRepository, TrustedContactRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();

            // Cross-cutting
            services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddScoped<INotificationService, SmsNotificationService>();

            return services;
        }
    }
}


