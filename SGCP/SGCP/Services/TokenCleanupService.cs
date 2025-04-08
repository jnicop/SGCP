using Microsoft.Extensions.Options;
using SGCP.Security.Configuration;

namespace SGCP.Services
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TokenCleanupService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromHours(6); // cada 6 horas

        public TokenCleanupService(IServiceProvider serviceProvider, IOptions<TokenCleanupSettings> settings, ILogger<TokenCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _interval = TimeSpan.FromMinutes(settings.Value.IntervalMinutes);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Token cleanup service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                try
                {
                    var deleted = await authService.PurgeExpiredRefreshTokensAsync();
                    _logger.LogInformation($"Token cleanup: {deleted} expired/revoked tokens removed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during token cleanup.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
