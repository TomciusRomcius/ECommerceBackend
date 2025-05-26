namespace ECommerce.Presentation.Common.Utils;

public class LoggerManager
{
    private static LoggerManager? _loggerManager;
    private readonly ILoggerFactory _loggerFactory;

    public LoggerManager()
    {
        _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
    }

    public static LoggerManager GetInstance()
    {
        if (_loggerManager == null) _loggerManager = new LoggerManager();

        return _loggerManager;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggerFactory.CreateLogger(categoryName);
    }
}