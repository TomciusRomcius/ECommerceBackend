namespace ECommerce.Common.Utils
{
    public class LoggerManager
    {
        private ILoggerFactory _loggerFactory;
        private static LoggerManager? _loggerManager;

        static public LoggerManager GetInstance()
        {
            if (_loggerManager == null)
            {
                _loggerManager = new LoggerManager();
            }

            return _loggerManager;
        }

        public LoggerManager()
        {
            _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggerFactory.CreateLogger(categoryName);
        }
    }
}