namespace SGCP.Services.Logger
{
    public class LoggingService<T> : ILoggingService<T>
    {
        private readonly ILogger<T> _logger;

        public LoggingService(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void Debug(string message, params object[] args) =>
            _logger.LogDebug(message, args);

        public void Info(string message, params object[] args) =>
            _logger.LogInformation(message, args);

        public void Warn(string message, params object[] args) =>
            _logger.LogWarning(message, args);

        public void Error(string message, params object[] args) =>
            _logger.LogError(message, args);

        public void Error(Exception ex, string message, params object[] args) =>
            _logger.LogError(ex, message, args);
    }

}
