using System.Diagnostics;
using System.Text;

namespace DependencyInjection.Services
{
    /*
	 * This class measures time intervals between events.
	 */
    public class StopwatchService
    {
        // Stopwatch used to track elapsed time
        private Stopwatch stopwatch = new Stopwatch();

        // Builds a string containing recorded lap info
        private StringBuilder builder = new StringBuilder();

        // Starts the stopwatch and records the first lap
        public void Start(string name)
        {
            ConsoleLogger.Instance.Log("Starting a new stopwatch");
            Lap(name);
            stopwatch.Start();
        }

        // Records a timestamp (lap) with the given name
        public void Lap(string name)
        {
            builder.Append("{").Append(name).Append(" ").Append(stopwatch.ElapsedTicks).Append("}");
        }

        // Returns all recorded laps as a single string
        public override string ToString()
        {
            return builder.ToString();
        }
    }
}
