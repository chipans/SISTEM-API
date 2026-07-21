using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SistemApi.Domain.Repositories;

namespace SistemApi.Infrastructure.BackgroundServices;

public class RefreshTokenCleanupService : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    private readonly IServiceProvider _serviceProvider;

    public RefreshTokenCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();

            await repository.DeleteExpiredAsync(DateTime.UtcNow);
            await Task.Delay(Interval, stoppingToken);
        }
    }
}

