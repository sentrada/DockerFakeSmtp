using FakeSmtpService.MessageStore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FakeSmtpService.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddFakeSmtp(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.TryAddSingleton<IFakeSmtp,FakeSmtp>();
        services.TryAddSingleton<IMessageRepository,InMemoryMessageRepository>();
        return services;
    }
}