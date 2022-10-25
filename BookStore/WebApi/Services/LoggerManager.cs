namespace WebApi.Services
{
    public class LoggerManager : ILoggerService
    {
        private readonly IEnumerable<ISingleLogger> _loggers;

        public LoggerManager(IEnumerable<ISingleLogger> loggers)
        {
            _loggers = loggers;
        }

        public void Log(string message)
        {
            foreach (var logger in _loggers)
                logger.Log(message);
        }
    }
}