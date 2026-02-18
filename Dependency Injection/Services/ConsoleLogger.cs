using System;

namespace DependencyInjection.Services
{
    /*
	 * The only change you should make to this class is to implement an interface.
	 *
	 * This class logs output to the console.
	 */
    public class ConsoleLogger : ILogger
    {
        // A single, shared instance (singleton pattern)
        private static ConsoleLogger instance = new ConsoleLogger();

        // Public property to access the single instance
        public static ConsoleLogger Instance { get { return instance; } }

        // Constructor ensures only one instance is ever created
        public ConsoleLogger()
        {
            if (instance != null)
            {
                throw new InvalidOperationException("Tried to create a second ConsoleLogger. That's bad.");
            }
        }

        // Writes the given message to the console
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
