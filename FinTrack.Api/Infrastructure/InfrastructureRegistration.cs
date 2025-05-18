using FinTrack.Api.Infrastructure.Implementations;
using FinTrack.Api.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrack.Api.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
            .AddTransient<IPasswordHasher, PasswordHasher>()
            .AddTransient<IJwtProvider, JwtProvider>();
    }
}
