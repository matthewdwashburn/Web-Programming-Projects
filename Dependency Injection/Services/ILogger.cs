namespace DependencyInjection.Services
{
    // Defines a simple logging interface
    public interface ILogger
    {
        // Logs a message to the desired output (e.g., console, file, etc.)
        public void Log(string message);
    }
}
