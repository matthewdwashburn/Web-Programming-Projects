using DependencyInjection.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DependencyInjection.Filters
{
    // Logs details of each HTTP request
    public class RequestLoggingFilter : IActionFilter
    {
        // Logger to record messages
        private readonly ILogger logger;

        // Constructor injects a logger
        public RequestLoggingFilter(ILogger logger)
        {
            this.logger = logger;
        }

        // Called after the action executes
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            var path = request.Path;
            var query = request.QueryString.Value;
            var method = request.Method;

            // Log the HTTP method, path, and query string
            logger.Log($"{method} {path} {query}");
        }

        // Called before the action executes
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Nothing needed here
        }
    }
}
