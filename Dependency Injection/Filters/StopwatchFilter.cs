using DependencyInjection.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace DependencyInjection.Filters
{
    /*
     * This class measures how long a request takes and adds timing info to response headers.
     */
    public class StopwatchFilter : IActionFilter
    {
        // Service for recording lap times
        private readonly StopwatchService watchService;

        // Tracks total request execution time
        private Stopwatch totalTimer;

        // Constructor injects the stopwatch service
        public StopwatchFilter(StopwatchService watchService)
        {
            this.watchService = watchService;
        }

        // Called after the action executes
        public void OnActionExecuted(ActionExecutedContext context)
        {
            watchService.Lap("Action Executed"); // Record final lap
            totalTimer.Stop(); // Stop the total timer

            // Add recorded timings to response headers
            context.HttpContext.Response.Headers.Append("stopwatch", new string[] { watchService.ToString() });
            context.HttpContext.Response.Headers.Append("totalTime", totalTimer.ElapsedMilliseconds.ToString());
        }

        // Called before the action executes
        public void OnActionExecuting(ActionExecutingContext context)
        {
            totalTimer = Stopwatch.StartNew(); // Start measuring total time
            watchService.Start("Action Executing"); // Record initial lap
        }
    }
}
